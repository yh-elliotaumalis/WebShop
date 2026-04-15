using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using Webshop.Domain.Entitites;
using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.UI;

namespace Webshop.Presentation.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;
    private readonly IVarukorgService _varukorgService;
    private readonly IKundService _kundService;

    private int _selectedProductIndex = 0;
    private readonly CurrencyService _currencyService = new();
    private decimal _rate;


    private List<Produkt> _products = new();

    public MainMenu(
        IProduktService productService,
        IKategoriService kategoriService,
        ILeverantörService leverantörService,
        IVarukorgService varukorgService)
        IKundService kundService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;
        _varukorgService = varukorgService;
        _kundService = kundService;

       
        _rate = _currencyService
            .GetRateAsync("SEK", "USD")
            .GetAwaiter()
            .GetResult();

        
        _options = new[]
        {
            "Kund",
            "Admin",
            "Avsluta"
        };
    }


    protected override void GetThreeProduckter()
    {
        _products = _productService
            .GetBestSellersAsync(3)
            .GetAwaiter()
            .GetResult()
            .ToList();
    }

    protected override void PopuleraProduker()
    {
        var products = _products;

        WriteCentered("=== Populära Produkter ===");
        Console.WriteLine();

        int spacing = 4;

        int boxWidth = Math.Min(
            40,
            (Console.WindowWidth - (products.Count - 1) * spacing) / products.Count
        );

        int totalWidth = boxWidth * products.Count + spacing * (products.Count - 1);
        int startX = Math.Max(0, (Console.WindowWidth - totalWidth) / 2);
        int startY = Console.CursorTop;

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];

            int x = startX + i * (boxWidth + spacing);
            int y = startY;

            Console.SetCursorPosition(x, y);
            Console.Write("┌" + new string('─', boxWidth - 2) + "┐");

            Console.SetCursorPosition(x, y + 1);
            Console.Write("│ " + p.Namn.PadRight(boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 2);
            Console.Write("│ " + $"Pris: {p.Pris:0.00} kr / Pris USD: {(p.Pris * _rate):0.00}".PadRight(boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 3);
            Console.Write("│ " + $"Färg: {p.Färg}".PadRight(boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 4);
            Console.Write("│ " + $"Kategori: {p.Kategori.Namn}".PadRight(boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 5);
            Console.Write("│ " + $"Lager: {p.LagerAntal}".PadRight(boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 6);
            Console.Write("│ " + new string(' ', boxWidth - 4) + " │");

            Console.SetCursorPosition(x, y + 7);

            string button = "[ Lägg i kundvagn ]";
            int padding = (boxWidth - 2 - button.Length) / 2;

            string buttonLine = "│"
                + new string(' ', padding)
                + button
                + new string(' ', boxWidth - 2 - padding - button.Length)
                + "│";

            if (i == _selectedProductIndex && _isProductFocused)
            {
                Console.BackgroundColor = Theme.MenuSelectedBg;
                Console.ForegroundColor = Theme.MenuSelectedText;
                Console.Write(buttonLine);
                Console.ResetColor();
            }
            else
            {
                Console.Write(buttonLine);
            }

            Console.SetCursorPosition(x, y + 8);
            Console.Write("└" + new string('─', boxWidth - 2) + "┘");
        }

        Console.SetCursorPosition(0, startY + 10);
    }

    protected override void MoveRight()
    {
        _selectedProductIndex = (_selectedProductIndex + 1) % _products.Count;
    }

    protected override void MoveLeft()
    {
        _selectedProductIndex = (_selectedProductIndex - 1 + _products.Count) % _products.Count;
    }

    protected override void HandleProductEnter()
    {
        var selectedProduct = _products[_selectedProductIndex];

        Console.SetCursorPosition(10, Console.WindowHeight - 2);
        Console.ForegroundColor = Theme.Message;
        Console.Write($"{selectedProduct.Namn} Lägg i kundvagn!");
        Console.ResetColor();

        Console.ReadKey();
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                new CustomerMenu(_productService, _varukorgService).ShowMenu("Kund Meny");
               
                new CustomerMenu(_productService, _rate).ShowMenu("Kund Meny");
                return false;

            case 1:
                var productHandler = new AdminProductHandler(_productService, _kategoriService, _leverantörService);
                var categoryHandler = new AdminCategoryHandler(_kategoriService);
                var customerHandler = new AdminCustomerHandler(_kundService);
                var leverantörHandler = new AdminLeverantörHandler(_leverantörService);
                var adminMenu = new AdminMenu(productHandler, categoryHandler, customerHandler, leverantörHandler);
                adminMenu.ShowMenu("Admin Meny");
                return false;

            case 2:
                return true;
        }

        return false;
    }
}