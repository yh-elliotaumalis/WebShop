using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class CartMenu : MenuBase
{
    private readonly IVarukorgService _varukorgService;
    private readonly IProduktService _produktService;
    private readonly CustomerProductHandler _handler;

    public CartMenu(IProduktService produktService, IVarukorgService varukorgService)
    {
        _produktService = produktService;
        _varukorgService = varukorgService;
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
                _varukorgService.Clear();
                return false;
            case 3:
                return true;
        }

        return false;
    }
    private void ShowCart()
    {
        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        Console.WriteLine("==========================");
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            Console.WriteLine($"Produkt namn: {produkt.Namn.PadRight(10)} Produkt antal: {item.Value}st Produkt pris: {produkt.Pris}SEK");
            produkter.Add(produkt);
        }
        var totalPris = _varukorgService.CalculateTotal(produkter);
        var moms = totalPris * 0.25m;
        var utanMoms = totalPris - moms;
        Console.WriteLine("------------------------");
        Console.WriteLine($"Exkl. moms:   {utanMoms}SEK");
        Console.WriteLine($"Moms (25%):   {moms}SEK");
        Console.WriteLine("------------------------");
        Console.WriteLine($"Totalt:       {totalPris}SEK");
        Console.WriteLine("==========================");
    }

    private void RemoveProduct()
    {
        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
    }

    protected override void DrawContent()
    {
        Console.Clear();
        ShowCart();
    }
}
