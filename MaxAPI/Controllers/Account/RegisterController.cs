using MaxAPI.Models.Accounts;
using MaxAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaxAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if (await _userService.ExistsAsync(registerUser.Username, registerUser.Email)) return StatusCode(409);

            await _userService.RegisterAsync(registerUser);

            return StatusCode(204);
        }
    }
}
