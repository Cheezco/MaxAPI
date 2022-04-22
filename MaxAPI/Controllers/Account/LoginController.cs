using MaxAPI.Contexts;
using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using MaxAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MainContext _context;

        public LoginController(MainContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Login(LoginUser loginUser)
        {
            var user = await AuthenticationUtils.AuthenticateUserAsync(_context, loginUser);

            if (user is null) return Unauthorized();

            return Ok(new { id = user.Id, username = user.Username, email = user.Email, token = AuthenticationUtils.GenerateJSONWebToken(user) });
        }
    }
}
