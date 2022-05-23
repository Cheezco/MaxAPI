using MaxAPI.Models.Doctors;
using MaxAPI.Models.Patients;
using System.Linq.Expressions;

namespace MaxAPI.Services.Interfaces
{
    public interface IPatientService
    {
        bool Exists(Expression<Func<Patient, bool>> predicate);
        bool Exists(string username, string email);
        Task<bool> ExistsAsync(string username, string email);
        Task<bool> ExistsAsync(Expression<Func<Patient, bool>> predicate);
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetPatientAsync(Expression<Func<Patient, bool>> predicate);
        Task<List<Patient>> GetAllAsync();
        Task<List<Patient>> GetAllAsync(Expression<Func<Patient, bool>> predicate);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
        Task DeleteAsync(Patient patient);
        Task<Patient?> GetPatientWithDoctor(int patientId);
    }
}
