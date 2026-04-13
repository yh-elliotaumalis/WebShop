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

        var leverantörIndex = ConsoleHelper.OptionPicker("Välj en leverantör: ", leverantörer.Select(l => l.Namn).ToList());
        var kategoriIndex = ConsoleHelper.OptionPicker("Välj en kategori: ", categories.Select(k => k.Namn).ToList());

        var leverantör = leverantörer.ElementAt(leverantörIndex);
        var kategori = categories.ElementAt(kategoriIndex);

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

        var productIndex = ConsoleHelper.OptionPicker("Välj en produkt att uppdatera: ", products.Select(p => p.Namn).ToList());
        var selectedProduct = products.ElementAt(productIndex);

        var product = await _productService.GetByIdAsync(selectedProduct.Id);

        var options = new List<string> { "Namn", "Beskrivning", "Pris", "Lagerantal", "Kategori", "Leverantör", "Färg", "Storlek" };
        var selectedOption = ConsoleHelper.OptionPicker("Vad vill du uppdatera? ", options);

        switch (selectedOption)
        {
            case 0:
                product.Namn = ConsoleHelper.ReadString("Nytt namn: ");
                break;
            case 1:
                product.Beskrivning = ConsoleHelper.ReadString("Ny beskrivning: ");
                break;
            case 2:
                product.Pris = ConsoleHelper.ReadDecimal("Nytt pris: ");
                break;
            case 3:
                product.LagerAntal = ConsoleHelper.ReadInt("Nytt lagerantal: ");
                break;
            case 4:
                var categories = await _kategoriService.GetAllAsync();
                var kategoriIndex = ConsoleHelper.OptionPicker("Välj en ny kategori: ", categories.Select(k => k.Namn).ToList());
                product.Kategori = categories.ElementAt(kategoriIndex);
                break;
            case 5:
                var leverantörer = await _leverantörService.GetAllAsync();
                var leverantörIndex = ConsoleHelper.OptionPicker("Välj en ny leverantör: ", leverantörer.Select(l => l.Namn).ToList());
                product.Leverantör = leverantörer.ElementAt(leverantörIndex);
                break;
            case 6:
                product.Färg = ConsoleHelper.ReadString("Ny färg: ");
                break;
            case 7:
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
        var productIndex = ConsoleHelper.OptionPicker("Välj en produkt att ta bort: ", products.Select(p => p.Namn).ToList());
        var selectedProduct = products.ElementAt(productIndex);

        await _productService.DeleteAsync(selectedProduct.Id);

        Console.WriteLine($"\nProdukten {selectedProduct.Namn} har tagits bort!");
        Console.ReadKey(true);
    }
}
