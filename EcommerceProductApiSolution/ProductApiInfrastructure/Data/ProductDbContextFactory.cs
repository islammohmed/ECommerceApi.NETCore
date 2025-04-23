using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            optionsBuilder.UseSqlServer("Server=.;Database=ProductDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ProductDbContext(optionsBuilder.Options);
        }
    }
}
