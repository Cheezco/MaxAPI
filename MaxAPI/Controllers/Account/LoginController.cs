using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Login(LoginUser loginUser)
        {
            var user = await _authenticationService.AuthenticateUserAsync(loginUser);

            if (user is null) return Unauthorized();

            return Ok(new { id = user.Id, username = user.Username, email = user.Email, token = AuthenticationUtils.GenerateJSONWebToken(user) });
        }
    }
}
