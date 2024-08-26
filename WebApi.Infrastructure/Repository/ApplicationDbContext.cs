using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Models;
using WebApi.Infrastructure.Repository.Interface;

namespace WebApi.Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IPasswordHasher<UserInfo> _passwordHasher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPasswordHasher<UserInfo> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<UserInfo> UserInfo { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync().Result;
        }

        public void SeedDataAsync()
        {
            // Ensure the database is created
            Database.EnsureCreated();

            if (!UserInfo.Any())
            {
                var user = new UserInfo { Username = "testuser", Email = "testuser@gmail.com" };
                user.PasswordHash = _passwordHasher.HashPassword(user, "1234");
                UserInfo.Add(user);
                SaveChanges();
            }
        }
    }
}
