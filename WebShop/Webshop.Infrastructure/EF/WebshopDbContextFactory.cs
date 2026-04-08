using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Webshop.Infrastructure.EF;

public class WebshopDbContextFactory : IDesignTimeDbContextFactory<WebshopDbContext>
{
    public WebshopDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<WebshopDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<WebshopDbContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new WebshopDbContext(optionsBuilder.Options);
    }
}
