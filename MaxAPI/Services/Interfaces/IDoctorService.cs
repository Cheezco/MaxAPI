using MaxAPI.Models.Doctors;
using System.Linq.Expressions;

namespace MaxAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        bool Exists(Expression<Func<Doctor, bool>> predicate);
        bool Exists(string username, string email);
        Task<bool> ExistsAsync(string username, string email);
        Task<bool> ExistsAsync(Expression<Func<Doctor, bool>> predicate);
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetDoctorAsync(Expression<Func<Doctor, bool>> predicate);
        Task<List<Doctor>> GetAllAsync();
        Task<List<Doctor>> GetAllAsync(Expression<Func<Doctor, bool>> predicate);
        Task AddAsync(Doctor patient);
        Task UpdateAsync(Doctor patient);
        Task DeleteAsync(int id);
        Task DeleteAsync(Doctor patient);
    }
}
