using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace WebShop.Presentation.MenuHandlers;

public class AdminProductHandler
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;

    public AdminProductHandler(IProduktService productService, IKategoriService kategoriService, ILeverantörService leverantörService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;
    }

    public async Task HandleAddProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till ny produkt ===\n");

        var categories = await _kategoriService.GetAllAsync();
        var leverantörer = await _leverantörService.GetAllAsync();

        var namn = ConsoleHelper.ReadString("Produktnamn: ");
        var description = ConsoleHelper.ReadString("Beskrivning: ");
        var color = ConsoleHelper.ReadString("Färg: ");
        var size = ConsoleHelper.ReadString("Storlek: ");
        var price = ConsoleHelper.ReadDecimal("Pris: ");
        var amount = ConsoleHelper.ReadInt("Lagerantal: ");

        var leverantör = MenuBase.NavigateList(leverantörer.ToList(), (items, index) =>
        {
            Console.WriteLine("Välj en leverantör:");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
        });
        if (leverantör == null) return;

        var kategori = MenuBase.NavigateList(categories.ToList(), (items, index) =>
        {
            Console.WriteLine("Välj en kategori:");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
        });
        if (kategori == null) return;

        var product = new Produkt
        {
            Id = Guid.NewGuid(),
            Namn = namn,
            Beskrivning = description,
            Pris = price,
            LagerAntal = amount,
            Kategori = kategori,
            Leverantör = leverantör,
            Färg = color,
            Storlek = size,
        };

        await _productService.AddAsync(product);

        Console.WriteLine($"\nProdukten {product.Namn} har lagts till i {kategori.Namn}!");
        Console.ReadKey(true);
    }

    public async Task HandleUpdateProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Uppdatera produkt ===\n");

        var products = await _productService.GetAllAsync();

        var selectedProduct = MenuBase.NavigateList(products.ToList(), (items, index) =>
        {
            Console.WriteLine("Välj en produkt att uppdatera:");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
        });
        if (selectedProduct == null) return;

        var product = await _productService.GetByIdAsync(selectedProduct.Id);

        var options = new List<string> { "Namn", "Beskrivning", "Pris", "Lagerantal", "Kategori", "Leverantör", "Färg", "Storlek" };
        var selectedField = MenuBase.NavigateList(options, (items, index) =>
        {
            Console.WriteLine("Vad vill du uppdatera?");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i]}");
        });
        if (selectedField == null) return;

        switch (selectedField)
        {
            case "Namn":
                product.Namn = ConsoleHelper.ReadString("Nytt namn: ");
                break;
            case "Beskrivning":
                product.Beskrivning = ConsoleHelper.ReadString("Ny beskrivning: ");
                break;
            case "Pris":
                product.Pris = ConsoleHelper.ReadDecimal("Nytt pris: ");
                break;
            case "Lagerantal":
                product.LagerAntal = ConsoleHelper.ReadInt("Nytt lagerantal: ");
                break;
            case "Kategori":
                var categories = await _kategoriService.GetAllAsync();
                var kategori = MenuBase.NavigateList(categories.ToList(), (items, index) =>
                {
                    Console.WriteLine("Välj en ny kategori:");
                    for (int i = 0; i < items.Count; i++)
                        Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
                });
                if (kategori != null) product.Kategori = kategori;
                break;
            case "Leverantör":
                var leverantörer = await _leverantörService.GetAllAsync();
                var leverantör = MenuBase.NavigateList(leverantörer.ToList(), (items, index) =>
                {
                    Console.WriteLine("Välj en ny leverantör:");
                    for (int i = 0; i < items.Count; i++)
                        Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
                });
                if (leverantör != null) product.Leverantör = leverantör;
                break;
            case "Färg":
                product.Färg = ConsoleHelper.ReadString("Ny färg: ");
                break;
            case "Storlek":
                product.Storlek = ConsoleHelper.ReadString("Ny storlek: ");
                break;
        }

        await _productService.UpdateAsync(product);

        Console.WriteLine($"\nProdukten {product.Namn} har uppdaterats!");
        Console.ReadKey(true);
    }

    public async Task HandleDeleteProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ta bort produkt ===\n");

        var products = await _productService.GetAllAsync();
        var selectedProduct = MenuBase.NavigateList(products.ToList(), (items, index) =>
        {
            Console.WriteLine("Välj en produkt att ta bort:");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
        });
        if (selectedProduct == null) return;

        await _productService.DeleteAsync(selectedProduct.Id);

        Console.WriteLine($"\nProdukten {selectedProduct.Namn} har tagits bort!");
        Console.ReadKey(true);
    }
}
