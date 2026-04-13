using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.MenuHandlers;

namespace HustlersAB.Admin.Menus;

public class CustomerMenu : MenuBase
{
    private CustomerProductHandler _handler;
    public CustomerMenu(IProduktService productService)
    {
        _handler = new CustomerProductHandler(productService);
        _options = new[] { "Handla produkter", "Varukorgen", "Tillbaka" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ShowCatalog();
                return false;

            case 1:
                Console.Clear();
                Console.WriteLine("Varukorgen kommer senare...");
                Console.ReadKey(true);
                return false;

            case 2:
                return true;
        }

        return false;
    }

    private void DrawCatalog(IEnumerable<IGrouping<string, Produkt>> groups, int selectedIndex)
    {
        var num = 0;
        foreach (var group in groups)
        {
            Console.WriteLine($"\n▼ {group.Key.ToUpper()}");
            foreach (var product in group)
            {
                var markering = num == selectedIndex ? "> " : "  ";
                Console.WriteLine($"{markering}[{num + 1}] {product.Namn.PadRight(20)} {product.Pris} kr");
                num++;
            }
        }
    }

    private void ShowCatalog()
    {

        var productList = _handler.GetAllProductsAsync().GetAwaiter().GetResult().ToList();
        var groups = productList.GroupBy(p => p.Kategori!.Namn);

        var vald = NavigateList(productList, (list, i) => DrawCatalog(groups, i));
        if (vald == null) return;
        var produkt = _handler.GetProductAsync(vald.Id).GetAwaiter().GetResult();

        ShowProductDetails(produkt);

    }

    private void ShowProductDetails(Produkt? product)
    {
        if (product == null) return;

        Console.Clear();

        Console.WriteLine($"{"Namn:".PadRight(20)}{product.Namn}");
        Console.WriteLine($"{"Beskrivning:".PadRight(20)}{product.Beskrivning}");
        Console.WriteLine($"{"Pris:".PadRight(20)}{product.Pris}");
        Console.WriteLine($"{"Kategori:".PadRight(20)}{product.Kategori?.Namn}");
        Console.WriteLine($"{"Léverantör:".PadRight(20)}{product.Leverantör?.Namn}");
        Console.WriteLine($"{"LagerAntal:".PadRight(20)}{product.LagerAntal}");

        while (Console.KeyAvailable) Console.ReadKey(true);
        while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }

    }
}
