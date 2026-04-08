using HustlersAB.Admin.MenuHandlers;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProductService _productService;

    public MainMenu(IProductService productService)
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
                var adminHandler = new AdminHandler(_productService); // eller vad din handler kräver
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
