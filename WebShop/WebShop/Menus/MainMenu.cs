using HustlersAB.Admin.MenuHandlers;
using System.Threading.Channels;
using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using WebShop.Presentation.UI;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;

    public int _selectedProductIndex = 0;
    public MainMenu(IProduktService productService, KategoriService kategoriService, LeverantörService leverantörService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;

        _options = new[] { "Kund", "Admin", "Avsluta" };
    }
 
    protected override void PopuleraProduker()
    {

        var products = _productService.GetBestSellersAsync(3)
         .GetAwaiter()
         .GetResult()
         .ToList();

        WriteCentered("=== Populära Produkter ===");
        Console.WriteLine();

        int boxWidth = 30;
        int spacing = 5;

        int totalWidth = (products.Count * boxWidth) + ((products.Count - 1) * spacing);
        int startX = (Console.WindowWidth - totalWidth) / 2;

        int y = Console.CursorTop + 1;

        for (int i = 0; i < products.Count; i++)
        {

            var p = products[i];
            int x = startX + i * (boxWidth + spacing);

            var box = new Box(x, y, boxWidth, 10, p.Namn);
            box.Draw();

            Console.SetCursorPosition(x + 2, y + 2);
            Console.Write($"Pris: {p.Pris} kr");

            Console.SetCursorPosition(x + 2, y + 3);
            Console.Write($"Färg: {p.Färg}");

            Console.SetCursorPosition(x + 2, y + 4);
            Console.Write($"Kategori: {p.Kategori?.Namn}");

            Console.SetCursorPosition(x + 2, y + 5);
            Console.Write($"Lager: {p.LagerAntal}");

            Console.SetCursorPosition(x + 2, y + 7);
            char key = (char)('A' + i);
            Console.Write($"[{key}] Köp");


        }
        Console.SetCursorPosition(0, y + 12);

    }
    protected override void HandleProductKey(ConsoleKey key)
    {
        var products = _productService
            .GetBestSellersAsync(3)
            .GetAwaiter()
            .GetResult()
            .ToList();

        int index = key - ConsoleKey.A;

        if (index >= 0 && index < products.Count)
        {
            var selectedProduct = products[index];

            Console.SetCursorPosition(10, Console.WindowHeight - 2);
            Console.ForegroundColor = Theme.Message;
            // Här kan du lägga till logik för att lägga produkten i varukorgen
            Console.Write($"{selectedProduct.Namn} har lagts till i Varukorgen!");
            Console.ResetColor();

            Console.ReadKey();
        }
    }

    //Console.WriteLine();

    //WriteCentered("=== Populära Produkter ===");
    //Console.WriteLine();
    //int i = 1;
    //foreach (var product in products)
    //{
    //    WriteCentered($"--- {product.Namn} ---");
    //    WriteCentered($"Saldo : {product.Pris} kr");
    //    WriteCentered($"Färg : {product.Färg}");
    //    WriteCentered($"Kategori : {product.Kategori?.Namn}");
    //    WriteCentered($"Lager : {product.LagerAntal}");
    //    Console.WriteLine();
    //    i++;
    //}



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

