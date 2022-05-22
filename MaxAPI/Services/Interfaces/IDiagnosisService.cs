using MaxAPI.Models.Patients;
using System.Linq.Expressions;

namespace MaxAPI.Services.Interfaces
{
    public interface IDiagnosisService
    {
        bool Exists(Expression<Func<Diagnosis, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<Diagnosis, bool>> predicate);
        Task<Diagnosis?> GetByIdAsync(int id);
        Task<Diagnosis?> GetDiagnosisAsync(Expression<Func<Diagnosis, bool>> predicate);
        Task<List<Diagnosis>> GetAllAsync();
        Task<List<Diagnosis>> GetAllAsync(Expression<Func<Diagnosis, bool>> predicate);
        Task<List<Diagnosis>> GetAllAsync(Patient patient);
        Task<bool> AddAsync(Diagnosis diagnosis);
        Task UpdateAsync(Diagnosis diagnosis);
        Task DeleteAsync(int id);
        Task DeleteAsync(Diagnosis diagnosis);
    }
}
