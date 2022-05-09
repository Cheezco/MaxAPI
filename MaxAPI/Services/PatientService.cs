using MaxAPI.Contexts;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly MainContext _context;

        public PatientService(MainContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Patient patient)
        {
            _context.Patients
                .Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = _context.Patients
                .FirstOrDefault(x => x.Id == id);

            if (patient is null) return;

            await DeleteAsync(patient);
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients
                .Remove(patient);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Expression<Func<Patient, bool>> predicate)
        {
            return _context.Patients
                .Any(predicate);
        }

        public bool Exists(string username, string email)
        {
            return _context.Patients
                .Any(x => x.Username == username && EF.Functions.Like(x.Email, $"%{email}%"));
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            return await _context.Patients
                .AnyAsync(x => x.Username == username && EF.Functions.Like(x.Email, $"%{email}%"));
        }

        public async Task<bool> ExistsAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await _context.Patients
                .AnyAsync(predicate);
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(x => x.Doctor)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetAllAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await _context.Patients
                .Include(x => x.Doctor)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Patient?> GetPatientAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await _context.Patients
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
