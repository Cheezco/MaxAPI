using MaxAPI.Contexts;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxAPI.Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly MainContext _context;

        public VaccinationService(MainContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Vaccination vaccination)
        {
            var patient = await _context.Patients
                .Include(x => x.Vaccinations)
                .FirstOrDefaultAsync(x => x.Id == vaccination.PatientId);

            if (patient is null) return false;

            _context.Vaccinations
                .Add(vaccination);
            if (patient.Vaccinations is null)
            {
                patient.Vaccinations = new List<Vaccination>();
            }
            patient.Vaccinations.Add(vaccination);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var vaccination = _context.Vaccinations
                .FirstOrDefault(x => x.Id == id);

            if (vaccination is null) return;

            await DeleteAsync(vaccination);
        }

        public async Task DeleteAsync(Vaccination vaccination)
        {
            _context.Vaccinations
                .Remove(vaccination);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Expression<Func<Vaccination, bool>> predicate)
        {
            return _context.Vaccinations
                .Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Vaccination, bool>> predicate)
        {
            return await _context.Vaccinations
                .AnyAsync(predicate);
        }

        public async Task<List<Vaccination>> GetAllAsync()
        {
            return await _context.Vaccinations
                .ToListAsync();
        }

        public async Task<List<Vaccination>> GetAllAsync(Expression<Func<Vaccination, bool>> predicate)
        {
            return await _context.Vaccinations
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<Vaccination>> GetAllAsync(Patient patient)
        {
            var foundPatient = await _context.Patients
               .Include(x => x.Vaccinations)
               .FirstOrDefaultAsync(x => x.Id == patient.Id);

            if (foundPatient is null) return new List<Vaccination>();

            return foundPatient.Vaccinations ?? new List<Vaccination>();
        }

        public async Task<Vaccination?> GetByIdAsync(int id)
        {
            return await _context.Vaccinations
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Vaccination?> GetVaccinationAsync(Expression<Func<Vaccination, bool>> predicate)
        {
            return await _context.Vaccinations
                .FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Vaccination vaccination)
        {
            _context.Entry(vaccination).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
