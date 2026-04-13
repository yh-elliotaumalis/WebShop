using HustlersAB.Admin.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Application.Services;
using Webshop.Infrastructure.EF;
using Webshop.Infrastructure.EF.Seeds;
using Webshop.Infrastructure.Repositories;
using Webshop.Presentation.Menus;

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

        var produktRepo = new ProduktRepository(db);
        var produktService = new ProduktService(produktRepo);

        var kategoriRepo = new KategoriRepository(db);
        var kategoriService = new KategoriService(kategoriRepo);

        var leverantörRepo = new LeverantörRepository(db);
        var leverantörService = new LeverantörService(leverantörRepo);

        var menu = new MainMenu(produktService, kategoriService, leverantörService);
        menu.ShowMenu("Välj meny");

    }
}
