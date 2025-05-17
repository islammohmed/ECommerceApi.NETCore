
using AuthenticationApi.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options) : DbContext(options)
    {
        
            public DbSet<AppUser> Users { get; set; }
        

    }
}
