using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Business.Interface;
using WebApi.Infrastructure.Models;
using WebApi.Infrastructure.Repository.Interface;
using WebApi.Utilities.Models;

namespace WebApi.Core.Business
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IPasswordHasher<UserInfo> _passwordHasher;
        public UserAccountManager(IApplicationDbContext applicationDbContext, IPasswordHasher<UserInfo> passwordHasher)
        {
            _applicationDbContext = applicationDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> IsUserExists(UserAccount userAccount)
        {
            bool isExists = await _applicationDbContext.UserInfo.AnyAsync(u => u.Username == userAccount.Username || u.Email == userAccount.Email);
            return isExists;
        }

        public async Task RegisterUser(UserAccount userAccount)
        {
            // Create a new user
            var user = new UserInfo
            {
                Username = userAccount.Username,
                Email = userAccount.Email
            };
            // Hash the password before saving
            user.PasswordHash = _passwordHasher.HashPassword(user, userAccount.Password);

            // Save the user to the database
            _applicationDbContext.UserInfo.Add(user);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyUserCredentials(UserAccount userAccount)
        {
            var user = await _applicationDbContext.UserInfo.SingleOrDefaultAsync(u => u.Username == userAccount.Username);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userAccount.Password) == PasswordVerificationResult.Failed)
                return false;
            else
                return true;
        }
    }
}
