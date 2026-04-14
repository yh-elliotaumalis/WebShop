using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class CartMenu : MenuBase
{
    private readonly IVarukorgService _varuorgService;
    private readonly IProduktService _produktService;
    private readonly CustomerProductHandler _handler;

    public CartMenu(IProduktService produktService, IVarukorgService varukorgService)
    {
        _produktService = produktService;
        _varuorgService = varukorgService;
        _options = new[] { "Ändra antal", "Tabort produkt", "Rensa varukorg", "Tillbaka" };
        _handler = new CustomerProductHandler(produktService);
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                return false;
            case 1:
                return false;
            case 2:
                return false;
            case 3:
                return true;
        }

        return false;
    }
    private void ShowCart()
    {
        var items = _varuorgService.GetItems();
        var produkter = new List<Produkt>();
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            Console.WriteLine($"Produkt namn: {produkt.Namn}\t Produkt antal: {item.Value}\t Produkt pris: {produkt.Pris}");
            produkter.Add(produkt);
        }
        var totalPris = _varuorgService.CalculateTotal(produkter);
        Console.WriteLine($"Total pris: {totalPris}");
    }
}
