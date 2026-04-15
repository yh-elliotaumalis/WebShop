using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.Menus;
using WebShop.Presentation.Validator;

namespace HustlersAB.Admin.Menus;

public class CustomerMenu : MenuBase
{
    private CustomerProductHandler _handler;
    private IVarukorgService _varukorgService;
    private IProduktService _produktService;
    private readonly IKundService _kundService;
    private readonly IFraktOmbudRepository _fraktOmbudRepository;

    public CustomerMenu(IProduktService productService, IVarukorgService varukorgService, IKundService kundService, IFraktOmbudRepository fraktOmbudRepository)
    {
        _produktService = productService;
        _varukorgService = varukorgService;
        _kundService = kundService;
        _fraktOmbudRepository = fraktOmbudRepository;
        _handler = new CustomerProductHandler(productService);
        _options = new[] { "Handla produkter", "Sök produkt", "Varukorgen", "Tillbaka" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ShowCatalog();
                return false;
            case 1:
                SearchCatalog();
                return false;

            case 2:
                new CartMenu(_produktService, _varukorgService, _fraktOmbudRepository, _kundService).ShowMenu("Varukorg");
                return false;

            case 3:
                return true;
        }

        return false;
    }
    private string? GetSearchInput()
    {
        Console.Clear();
        Console.Write("Sök produkt: ");
        var input = Console.ReadLine() ?? "";

        var error = new SearchValidator().Validate(input);
        if (error != null)
        {
            Console.WriteLine(error);
            Console.ReadKey(true);
            return null;
        }
        return input;
    }

    private void DrawSearchResults(List<Produkt> list, string query, int selectedIndex)
    {
        Console.WriteLine($"Sök resultat för {query}\n");
        for (int i = 0; i < list.Count; i++)
        {
            var markering = i == selectedIndex ? "> " : "  ";
            Console.WriteLine($"{markering}{list[i].Namn.PadRight(20)} {list[i].Pris} kr");
        }
    }

    private void SearchCatalog()
    {
        var input = GetSearchInput();
        if (input == null) return;

        var list = _handler.SearchProductAsync(input).GetAwaiter().GetResult().ToList();
        if (!list.Any())
        {
            Console.WriteLine("Inga produkter hittades.");
            Console.ReadKey(true);
            return;
        }

        var vald = NavigateList(list, (l, i) => DrawSearchResults(l, input, i));
        if (vald == null) return;

        var produkt = _handler.GetProductAsync(vald.Id).GetAwaiter().GetResult();
        ShowProductDetails(produkt);
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

        while (true)
        {
            var vald = NavigateList(productList, (list, i) => DrawCatalog(groups, i));
            if (vald == null) return;
            var produkt = _handler.GetProductAsync(vald.Id).GetAwaiter().GetResult();
            ShowProductDetails(produkt);
        }
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

        Console.WriteLine("\nEnter = Lägg i varukorg | Escape = Tillbaka");

        while (Console.KeyAvailable) Console.ReadKey(true);
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape) break;
            if (key == ConsoleKey.Enter)
            {
                _varukorgService.AddProduct(product.Id, 1);
                Console.WriteLine("Produkt tillagd i varukorgen!");
            }
        }
    }
}
