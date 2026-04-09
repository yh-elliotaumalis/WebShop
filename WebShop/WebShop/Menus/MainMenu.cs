using HustlersAB.Admin.MenuHandlers;
using Webshop.Application.Interfaces;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;

    public MainMenu(IProduktService productService)
    {
        _productService = productService;

        _options = new[] { "Kund", "Admin", "Avsluta" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                new CustomerMenu().ShowMenu("Kund Meny");
                return false;

            case 1:
                var adminHandler = new AdminHandler(_productService);
                var adminMenu = new AdminMenu(adminHandler);
                adminMenu.ShowMenu("Admin Meny");
                return false;

            case 2:
                Environment.Exit(0);
                return true;
        }

        return false;
    }
}
