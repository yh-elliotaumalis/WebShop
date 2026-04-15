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
        db.Database.Migrate();
        WebShopSeeder.Seed(db);

        var produktRepo = new ProduktRepository(db);
        var produktService = new ProduktService(produktRepo);

        var kategoriRepo = new KategoriRepository(db);
        var kategoriService = new KategoriService(kategoriRepo);

        var leverantörRepo = new LeverantörRepository(db);
        var leverantörService = new LeverantörService(leverantörRepo);

        var varukorgService = new VarukorgService();
        var kundRepo = new KundRepository(db);
        var kundService = new KundService(kundRepo);
        var fraktOmbudRepo = new FraktOmbudRepository(db);
        var orderRepo = new OrderRepository(db);
        var orderService = new OrderService(orderRepo);

        var menu = new MainMenu(produktService, kategoriService, leverantörService, varukorgService, kundService, orderService, fraktOmbudRepo);
        menu.ShowMenu("Välj meny");
    }
}
