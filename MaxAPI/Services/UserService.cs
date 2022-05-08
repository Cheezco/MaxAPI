using MaxAPI.Contexts;
using MaxAPI.Models.Accounts;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxAPI.Services
{
    public class UserService : IUserService
    {
        private readonly MainContext _context;

        public UserService(MainContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user is null) return;

            await DeleteAsync(user);
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            return await _context.Users.AnyAsync(x => x.Username == username &&
                EF.Functions.Like(x.Email, $"%{email}%"));
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
