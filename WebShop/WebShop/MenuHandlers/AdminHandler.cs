using System.Drawing;
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



}
