using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace HustlersAB.Admin.MenuHandlers;

public class AdminHandler
{
    private readonly IProduktService _productService;

    public AdminHandler(IProduktService productService)
    {
        _productService = productService;
    }

    public async Task HandleAddProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till ny produkt ===\n");

        var selectedSubCategory = await _productService.GetAllAsync();
        if (selectedSubCategory == null) return;

        var name = ("Produktnamn: ");
        var description = ("Beskrivning: ");
        var price = ("Pris: ", minValue: 0.01m);
        var qty = ("Lagersaldo: ", minValue: 0);

        var product = new Produkt
        {
            Id = Guid.NewGuid(),
            Namn = name,
            Beskrivning = description,
        };

        await _productService.AddAsync(product);

        Console.WriteLine($"\nProdukten {product} har lagts till i {selectedSubCategory}!");

        Console.ReadKey(true);
    }



}
