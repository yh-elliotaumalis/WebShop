using System.Drawing;
using System.Runtime.InteropServices;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;

namespace HustlersAB.Admin.MenuHandlers;

public class AdminHandler
{
    private readonly IProduktService _productService;
    private readonly IKategoriService _kategoriService;
    private readonly ILeverantörService _leverantörService;

    public AdminHandler(IProduktService productService, IKategoriService kategoriService, ILeverantörService leverantörService)
    {
        _productService = productService;
        _kategoriService = kategoriService;
        _leverantörService = leverantörService;
    }
    public static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
                return result;
            Console.WriteLine("Ogiltigt tal. Försök igen.");
        }
    }
    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal result))
                return result;
            Console.WriteLine("Ogiltigt tal. Försök igen.");
        }
    }

    public static int OptionPicker(string prompt, List<string> options)
    {
        Console.WriteLine(prompt);

        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {options[i]}");
        }

        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= options.Count)
            {
                return selectedIndex - 1;
            }
            Console.WriteLine("Ogiltigt val. Försök igen.");
        }
    }

    public async Task HandleAddProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till ny produkt ===\n");

        var categories = await _kategoriService.GetAllAsync();
        var leverantörer = await _leverantörService.GetAllAsync();

        var namn = ReadString("Produktnamn: ");

        var description = ReadString("Beskrivning: ");

        var color = ReadString("Färg: ");

        var size = ReadString("Storlek: ");

        var price = ReadDecimal("Pris: ");

        var amount = ReadInt("Lagerantal: ");

        var leverantörIndex = OptionPicker("Välj en leverantör: ", leverantörer.Select(l => l.Namn).ToList());
        var kategoriIndex = OptionPicker("Välj en kategori: ", categories.Select(k => k.Namn).ToList());

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

        var productIndex = OptionPicker("Välj en produkt att uppdatera: ", products.Select(p => p.Namn).ToList());
        var selectedProduct = products.ElementAt(productIndex);

        var product = await _productService.GetByIdAsync(selectedProduct.Id);

        var options = new List<string> { "Namn", "Beskrivning", "Pris", "Lagerantal", "Kategori", "Leverantör", "Färg", "Storlek" };
        var selectedOption = OptionPicker("Vad vill du uppdatera? ", options);

        switch (selectedOption)
        {
            case 0:
                product.Namn = ReadString("Nytt namn: ");
                break;
            case 1:
                product.Beskrivning = ReadString("Ny beskrivning: ");
                break;
            case 2:
                product.Pris = ReadDecimal("Nytt pris: ");
                break;
            case 3:
                product.LagerAntal = ReadInt("Nytt lagerantal: ");
                break;
            case 4:
                var categories = await _kategoriService.GetAllAsync();
                var kategoriIndex = OptionPicker("Välj en ny kategori: ", categories.Select(k => k.Namn).ToList());
                product.Kategori = categories.ElementAt(kategoriIndex);
                break;
            case 5:
                var leverantörer = await _leverantörService.GetAllAsync();
                var leverantörIndex = OptionPicker("Välj en ny leverantör: ", leverantörer.Select(l => l.Namn).ToList());
                product.Leverantör = leverantörer.ElementAt(leverantörIndex);
                break;
            case 6:
                product.Färg = ReadString("Ny färg: ");
                break;
            case 7:
                product.Storlek = ReadString("Ny storlek: ");
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
        var productIndex = OptionPicker("Välj en produkt att ta bort: ", products.Select(p => p.Namn).ToList());
        var selectedProduct = products.ElementAt(productIndex);
        await _productService.DeleteAsync(selectedProduct.Id);
        Console.WriteLine($"\nProdukten {selectedProduct.Namn} har tagits bort!");
        Console.ReadKey(true);
    }
}
