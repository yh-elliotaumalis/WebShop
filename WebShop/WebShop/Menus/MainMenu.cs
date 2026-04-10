using HustlersAB.Admin.MenuHandlers;
using Webshop.Application.Interfaces;
using Webshop.Application.Services;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;

    public MainMenu(IProduktService productService, KategoriService kategoriService, LeverantörService leverantörService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;

        _options = new[] { "Kund", "Admin", "Avsluta" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                new CustomerMenu(_productService).ShowMenu("Kund Meny");
                return false;

            case 1:
                var adminHandler = new AdminHandler(_productService, _kategoriService, _leverantörService);
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

