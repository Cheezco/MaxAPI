using MaxAPI.Contexts;
using MaxAPI.Models.Accounts;
using MaxAPI.Models.Doctors;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace MaxAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly MainContext _context;

        public RegistrationService(MainContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterUser registerUser)
        {
            if (await ExistsAsync(registerUser)) return false;

            var salt = HashingUtils.CreateSalt(128);
            var hashedPassword = HashingUtils.GetArgon2Hash(registerUser.Password, salt);
            var user = new User()
            {
                Password = hashedPassword,
                Salt = salt,
                Email = registerUser.Email,
                Username = registerUser.Username,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Role = registerUser.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterPatient registerPatient)
        {
            if (await ExistsAsync(registerPatient)) return false;

            var salt = HashingUtils.CreateSalt(128);
            var hashedPassword = HashingUtils.GetArgon2Hash(registerPatient.Password, salt);
            Doctor? doctor = null;
            if (registerPatient.DoctorId is not null)
            {
                doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == registerPatient.DoctorId);
            }
            var patient = new Patient()
            {
                Password = hashedPassword,
                Salt = salt,
                Email = registerPatient.Email,
                Username = registerPatient.Username,
                FirstName = registerPatient.FirstName,
                LastName = registerPatient.LastName,
                PersonalCode = registerPatient.PersonalCode,
                Role = Enums.Role.Patient,
                Doctor = doctor,
                Vaccinations = new List<Vaccination>()
            };

            _context.Users.Add(patient);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDoctor registerDoctor)
        {
            if (await ExistsAsync(registerDoctor)) return false;

            var salt = HashingUtils.CreateSalt(128);
            var hashedPassword = HashingUtils.GetArgon2Hash(registerDoctor.Password, salt);
            var doctor = new Doctor()
            {
                Password = hashedPassword,
                Salt = salt,
                Email = registerDoctor.Email,
                Username = registerDoctor.Username,
                FirstName = registerDoctor.FirstName,
                LastName = registerDoctor.LastName,
                Role = Enums.Role.Doctor,
                Education = string.Empty
            };

            _context.Users.Add(doctor);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> ExistsAsync(RegisterUser user)
        {
            return await _context.Users.AnyAsync(x => x.Username == user.Username &&
                EF.Functions.Like(x.Email, $"%{user.Email}%"));
        }
    }
}
