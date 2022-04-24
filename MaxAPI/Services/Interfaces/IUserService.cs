using MaxAPI.Models.Accounts;
using System.Linq.Expressions;

namespace MaxAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(string username, string email);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task DeleteAsync(User user);
        Task RegisterAsync(RegisterUser registerUser);
    }
}
