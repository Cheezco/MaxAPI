using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MaxAPI.Attributes;
using MaxAPI.Enums;
using MaxAPI.Utils;
using MaxAPI.Models.Doctors;

namespace MaxAPI.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _patientService.GetAllAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            var role = ClaimUtils.GetRole(HttpContext);
            int claimId = ClaimUtils.GetId(HttpContext);

            if (patient is null)
            {
                return NotFound();
            }

            if (claimId != id || role != Role.Admin)
            {
                return Unauthorized();
            }

            return patient;
        }

        [HttpGet("doctor/{id}")]
        [Authorize]
        [AuthorizationRole(Role.Patient)]
        public async Task<ActionResult<Doctor>> GetPatientsDoctor(int id)
        {
            var patient = await _patientService.GetPatientWithDoctor(id);
            var claimId = ClaimUtils.GetId(HttpContext);

            if (id != claimId)
            {
                return Unauthorized();
            }

            if (patient is null || patient.Doctor is null)
            {
                return NotFound();
            }

            return patient.Doctor;
        }

        [HttpPut("{id}")]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            try
            {
                await _patientService.UpdateAsync(patient);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            await _patientService.AddAsync(patient);

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [AuthorizationRole(Role.Admin)]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient is null)
            {
                return NotFound();
            }

            await _patientService.DeleteAsync(patient);

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _patientService.Exists(x => x.Id == id);
        }
    }
}
