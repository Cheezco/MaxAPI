#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaxAPI.Contexts;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using MaxAPI.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MaxAPI.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationService _vaccinationService;
        private readonly IPatientService _patientService;

        public VaccinationsController(IVaccinationService vaccinationService, IPatientService patientService)
        {
            _vaccinationService = vaccinationService;
            _patientService = patientService;
        }

        // GET: api/Vaccinations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Vaccination>>> GetVaccinations()
        {
            return await _vaccinationService.GetAllAsync();
        }

        [HttpGet("patient/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Vaccination>>> GetPatientVaccinations(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            var role = ClaimUtils.GetRole(HttpContext);
            var claimId = ClaimUtils.GetId(HttpContext);

            if (patient is null)
            {
                return NotFound();
            }
            if (role == Role.Patient && patient.Id != claimId)
            {
                return Unauthorized();
            }

            return await _vaccinationService.GetAllAsync(patient);
        }

        // GET: api/Vaccinations/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Vaccination>> GetVaccination(int id)
        {
            var vaccination = await _vaccinationService.GetByIdAsync(id);
            var role = ClaimUtils.GetRole(HttpContext);
            var claimId = ClaimUtils.GetId(HttpContext);

            if (vaccination == null)
            {
                return NotFound();
            }

            if (role == Role.Patient)
            {
                var patient = await _patientService.GetByIdAsync(claimId);

                if (patient is null)
                {
                    return BadRequest();
                }

                var patientVaccines = await _vaccinationService.GetAllAsync(patient);

                if (!patientVaccines.Any(x => x.Id == id))
                {
                    return Unauthorized();
                }
            }

            return vaccination;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutVaccination(int id, Vaccination vaccination)
        {
            if (id != vaccination.Id)
            {
                return BadRequest();
            }

            try
            {
                await _vaccinationService.UpdateAsync(vaccination);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccinationExists(id))
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
        public async Task<ActionResult<Vaccination>> PostVaccination(Vaccination vaccination)
        {
            bool created = await _vaccinationService.AddAsync(vaccination);

            if (!created)
            {
                return StatusCode(409);
            }


            return CreatedAtAction("GetVaccination", new { id = vaccination.Id }, vaccination);
        }

        // DELETE: api/Vaccinations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVaccination(int id)
        {
            var vaccination = await _vaccinationService.GetByIdAsync(id);
            if (vaccination is null)
            {
                return NotFound();
            }

            await _vaccinationService.DeleteAsync(vaccination);

            return NoContent();
        }

        private bool VaccinationExists(int id)
        {
            return _vaccinationService.Exists(x => x.Id == id);
        }
    }
}
