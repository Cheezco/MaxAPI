using MaxAPI.Contexts;
using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using MaxAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaxAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MainContext _context;

        public RegisterController(MainContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerUser.Username && EF.Functions.Like(x.Email, $"%{registerUser.Email}%"))) return StatusCode(409);

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
                PersonalCode = registerUser.PersonalCode,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }
    }
}
