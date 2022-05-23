#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaxAPI.Contexts;
using MaxAPI.Models.Doctors;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using MaxAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using MaxAPI.Attributes;

namespace MaxAPI.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: api/Doctors
        [HttpGet]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            return await _doctorService.GetAllAsync();
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            var role = ClaimUtils.GetRole(HttpContext);
            var claimId = ClaimUtils.GetId(HttpContext);

            if (doctor == null)
            {
                return NotFound();
            }
            if (claimId != id || role != Role.Admin)
            {
                return Unauthorized();
            }

            return doctor;
        }

        [HttpPut("{id}")]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }

            try
            {
                await _doctorService.UpdateAsync(doctor);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            await _doctorService.AddAsync(doctor);

            return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            await _doctorService.DeleteAsync(doctor);

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return _doctorService.Exists(x => x.Id == id);
        }
    }
}
