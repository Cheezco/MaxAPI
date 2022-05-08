using MaxAPI.Contexts;
using MaxAPI.Models.Doctors;
using MaxAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly MainContext _context;

        public DoctorService(MainContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Doctor doctor)
        {
            _context.Doctors
                .Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = _context.Doctors
                .FirstOrDefault(x => x.Id == id);

            if (doctor is null) return;

            await DeleteAsync(doctor);
        }

        public async Task DeleteAsync(Doctor doctor)
        {
            _context.Doctors
                .Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Expression<Func<Doctor, bool>> predicate)
        {
            return _context.Doctors
                .Any(predicate);
        }

        public bool Exists(string username, string email)
        {
            return _context.Doctors
                .Any(x => x.Username == username && EF.Functions.Like(x.Email, $"%{email}%"));
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            return await _context.Doctors
                .AnyAsync(x => x.Username == username && EF.Functions.Like(x.Email, $"%{email}%"));
        }

        public async Task<bool> ExistsAsync(Expression<Func<Doctor, bool>> predicate)
        {
            return await _context.Doctors
                .AnyAsync(predicate);
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetAllAsync(Expression<Func<Doctor, bool>> predicate)
        {
            return await _context.Doctors
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Doctor?> GetDoctorAsync(Expression<Func<Doctor, bool>> predicate)
        {
            return await _context.Doctors.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
