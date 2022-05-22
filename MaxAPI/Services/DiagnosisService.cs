using MaxAPI.Contexts;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxAPI.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly MainContext _context;

        public DiagnosisService(MainContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(Diagnosis diagnosis)
        {
            var patient = await _context.Patients
                .Include(x => x.Diagnoses)
                .FirstOrDefaultAsync(x => x.Id == diagnosis.PatientId);

            if (patient is null) return false;

            _context.Diagnoses
                .Add(diagnosis);
            if (patient.Diagnoses is null)
            {
                patient.Diagnoses = new List<Diagnosis>();
            }
            patient.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var diagnosis = _context.Diagnoses
                .FirstOrDefault(x => x.Id == id);

            if (diagnosis is null) return;

            await DeleteAsync(diagnosis);
        }

        public async Task DeleteAsync(Diagnosis diagnosis)
        {
            _context.Diagnoses
                .Remove(diagnosis);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Expression<Func<Diagnosis, bool>> predicate)
        {
            return _context.Diagnoses
                .Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Diagnosis, bool>> predicate)
        {
            return await _context.Diagnoses
                .AnyAsync(predicate);
        }

        public async Task<List<Diagnosis>> GetAllAsync()
        {
            return await _context.Diagnoses
               .ToListAsync();
        }

        public async Task<List<Diagnosis>> GetAllAsync(Expression<Func<Diagnosis, bool>> predicate)
        {
            return await _context.Diagnoses
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<Diagnosis>> GetAllAsync(Patient patient)
        {
            var foundPatient = await _context.Patients
               .Include(x => x.Vaccinations)
               .FirstOrDefaultAsync(x => x.Id == patient.Id);

            if (foundPatient is null) return new List<Diagnosis>();

            return foundPatient.Diagnoses ?? new List<Diagnosis>();

        }

        public async Task<Diagnosis?> GetByIdAsync(int id)
        {
            return await _context.Diagnoses
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Diagnosis?> GetDiagnosisAsync(Expression<Func<Diagnosis, bool>> predicate)
        {
            return await _context.Diagnoses
                 .FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Diagnosis diagnosis)
        {
            _context.Entry(diagnosis).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
