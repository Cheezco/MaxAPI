using MaxAPI.Contexts;
using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MaxAPI.Utils
{
    public class AuthenticationUtils
    {
        public static string GenerateJSONWebToken(User user)
        {
            var signingKey = "A31kSjhFw1-pJupaTMd-pYdZmkSwAC7v5JPVa1wfcYHSKnpdZmPUyi94i4fYxu5uZpNi8ugaWuJAK9Zr79SjtA";
            var securityKey = new SymmetricSecurityKey(Base64Url.Decode(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken("max",
                "max",
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static async Task<User?> AuthenticateUserAsync(MainContext context, LoginUser loginUser)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == loginUser.LoginName);

            if (user is null) return null;

            return HashingUtils.VerifyArgon2(loginUser.Password, user) ? user : null;
        }
    }
}
