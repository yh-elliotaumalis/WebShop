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
        Console.Clear();
        var products = _handler.GetAllProductsAsync().GetAwaiter().GetResult();
        var productList = products.ToList();
        var groups = productList.GroupBy(p => p.Kategori!.Namn);
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            DrawCatalog(groups, selectedIndex);
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(productList.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    var vald = productList[selectedIndex];
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }

    }

}
