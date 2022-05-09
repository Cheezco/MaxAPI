using MaxAPI.Models.Patients;
using System.Linq.Expressions;

namespace MaxAPI.Services.Interfaces
{
    public interface IVaccinationService
    {
        bool Exists(Expression<Func<Vaccination, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<Vaccination, bool>> predicate);
        Task<Vaccination?> GetByIdAsync(int id);
        Task<Vaccination?> GetVaccinationAsync(Expression<Func<Vaccination, bool>> predicate);
        Task<List<Vaccination>> GetAllAsync();
        Task<List<Vaccination>> GetAllAsync(Expression<Func<Vaccination, bool>> predicate);
        Task<List<Vaccination>> GetAllAsync(Patient patient);
        Task<bool> AddAsync(Vaccination vaccination);
        Task UpdateAsync(Vaccination vaccination);
        Task DeleteAsync(int id);
        Task DeleteAsync(Vaccination vaccination);
    }
}
