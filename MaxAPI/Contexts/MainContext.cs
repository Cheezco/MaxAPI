using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace MaxAPI.Contexts
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainContext(DbContextOptions<MainContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
