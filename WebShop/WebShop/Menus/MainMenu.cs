using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using WebShop.Presentation.MenuHandlers;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;

   
    private readonly CurrencyService _currencyService = new();
    private decimal _rate;

    public MainMenu(IProduktService productService, KategoriService kategoriService, LeverantörService leverantörService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;

       
        _rate = _currencyService
            .GetRateAsync("SEK", "USD")
            .GetAwaiter()
            .GetResult();

        _options = new[] { "Kund", "Admin", "Avsluta" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
               
                new CustomerMenu(_productService, _rate).ShowMenu("Kund Meny");
                return false;

            case 1:
                var productHandler = new AdminProductHandler(_productService, _kategoriService, _leverantörService);
                var adminMenu = new AdminMenu(productHandler);
                adminMenu.ShowMenu("Admin Meny");
                return false;

            case 2:
                Environment.Exit(0);
                return true;
        }

        return false;
    }
}