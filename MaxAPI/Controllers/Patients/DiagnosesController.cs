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
    public class DiagnosesController : ControllerBase
    {
        private readonly IDiagnosisService _diagnosisService;
        private readonly IPatientService _patientService;

        //private readonly MainContext _context;

        public DiagnosesController(IDiagnosisService diagnosisService, IPatientService patientService)
        {
            _diagnosisService = diagnosisService;
            _patientService = patientService;
        }

        // GET: api/Diagnoses
        [HttpGet]
        [Authorize]

        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetDiagnoses()
        {
            return await _diagnosisService.GetAllAsync();
        }
        [HttpGet("patient/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetPatientDiagnoses(int id)
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

            return await _diagnosisService.GetAllAsync(patient);
        }

        // GET: api/Diagnoses/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Diagnosis>> GetDiagnosis(int id)
        {
            var diagnosis = await _diagnosisService.GetByIdAsync(id);
            var role = ClaimUtils.GetRole(HttpContext);
            var claimId = ClaimUtils.GetId(HttpContext);

            if (diagnosis == null)
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

                var patientDiagnoses = await _diagnosisService.GetAllAsync(patient);

                if (!patientDiagnoses.Any(x => x.Id == id))
                {
                    return Unauthorized();
                }
            }
            return diagnosis;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDiagnosis(int id, Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return BadRequest();
            }

            try
            {
                await _diagnosisService.UpdateAsync(diagnosis);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiagnosisExists(id))
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
        public async Task<ActionResult<Diagnosis>> PostDiagnosis(Diagnosis diagnosis)
        {
            bool created = await _diagnosisService.AddAsync(diagnosis);

            if (!created)
            {
                return StatusCode(409);
            }


            return CreatedAtAction("GetDiagnosis", new { id = diagnosis.Id }, diagnosis);
        }

        // DELETE: api/Diangoses/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDiagnosis(int id)
        {
            var diagnosis = await _diagnosisService.GetByIdAsync(id);
            if (diagnosis is null)
            {
                return NotFound();
            }

            await _diagnosisService.DeleteAsync(diagnosis);

            return NoContent();
        }


        private bool DiagnosisExists(int id)
        {
            return _diagnosisService.Exists(x => x.Id == id);
        }

    }
}
