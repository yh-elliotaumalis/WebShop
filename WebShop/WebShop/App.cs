using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Infrastructure.EF;
using Webshop.Infrastructure.EF.Seeds;

namespace WebShop.Presentation;

public class App
{
    public static void Run()
    {
        var config = new ConfigurationBuilder()
             .AddUserSecrets<App>()
             .Build();

        var services = new ServiceCollection();
        services.AddDbContext<WebshopDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        var servicesProvider = services.BuildServiceProvider();

        using var db = servicesProvider.GetRequiredService<WebshopDbContext>();
        WebShopSeeder.Seed(db);

    }
}
