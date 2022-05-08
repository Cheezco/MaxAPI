using MaxAPI.Models.Accounts;
using MaxAPI.Models.Doctors;
using MaxAPI.Models.Patients;
using MaxAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegisterController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            bool registered = await _registrationService.RegisterAsync(registerUser);

            return registered ? StatusCode(204) : StatusCode(409);
        }

        [HttpPost("patient")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterPatient registerPatient)
        {
            bool registered = await _registrationService.RegisterAsync(registerPatient);

            return registered ? StatusCode(204) : StatusCode(409);
        }

        [HttpPost("doctor")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDoctor registerDoctor)
        {
            bool registered = await _registrationService.RegisterAsync(registerDoctor);

            return registered ? StatusCode(204) : StatusCode(409);
        }
    }
}
