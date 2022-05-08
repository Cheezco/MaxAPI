using MaxAPI.Models.Accounts;
using MaxAPI.Models.Doctors;
using MaxAPI.Models.Patients;

namespace MaxAPI.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<bool> RegisterAsync(RegisterUser registerUser);
        Task<bool> RegisterAsync(RegisterPatient registerPatient);
        Task<bool> RegisterAsync(RegisterDoctor registerDoctor);
    }
}
