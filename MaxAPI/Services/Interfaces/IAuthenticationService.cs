using MaxAPI.Models;
using MaxAPI.Models.Accounts;

namespace MaxAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateJSONWebToken(User user);
        Task<User?> AuthenticateUserAsync(LoginUser loginUser);
    }
}
