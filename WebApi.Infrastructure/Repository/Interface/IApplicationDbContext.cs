using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Models;

namespace WebApi.Infrastructure.Repository.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<UserInfo> UserInfo { get; set; }
        Task<int> SaveChangesAsync();
        void SeedDataAsync();
    }
}
