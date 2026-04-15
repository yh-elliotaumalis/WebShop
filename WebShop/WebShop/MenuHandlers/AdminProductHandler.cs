using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.Validator;

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
        if (!categories.Any())
        {
            Console.WriteLine("Inga kategorier finns. Skapa en kategori innan du lägger till produkter.");
            Console.ReadKey(true);
            return;
        }
        var leverantörer = await _leverantörService.GetAllAsync();
        if (!leverantörer.Any())
        {
            Console.WriteLine("Inga leverantörer finns. Skapa en leverantör innan du lägger till produkter.");
            Console.ReadKey(true);
            return;
        }

        var namn = ConsoleHelper.ReadString("Produktnamn: ");
        if (!ProductValidator.ValidateProductName(namn))
        {
            Console.WriteLine("Ogiltigt produktnamn. Det måste vara minst 3 tecken långt.");
            Console.ReadKey(true);
            return;
        }
        var description = ConsoleHelper.ReadString("Beskrivning: ");
        if (!ProductValidator.ValidateProductDescription(description))
        {
            Console.WriteLine("Ogiltig beskrivning. Den måste vara minst 10 tecken lång.");
            Console.ReadKey(true);
            return;
        }
        var color = ConsoleHelper.ReadString("Färg: ");
        if (!ProductValidator.ValidateProductColor(color))
        {
            Console.WriteLine("Ogiltig färg.");
            Console.ReadKey(true);
            return;
        }
        var size = ConsoleHelper.ReadString("Storlek: ");
        if (!ProductValidator.ValidateProductSize(size))
        {
            Console.WriteLine("Ogiltig storlek.");
            Console.ReadKey(true);
            return;
        }
        var price = ConsoleHelper.ReadDecimal("Pris: ");
        if (!ProductValidator.ValidateProductPrice(price))
        {
            Console.WriteLine("Ogiltigt pris. Det måste vara ett positivt tal.");
            Console.ReadKey(true);
            return;
        }
        var amount = ConsoleHelper.ReadInt("Lagerantal: ");
        if (!ProductValidator.ValidateProductStock(amount))
        {
            Console.WriteLine("Ogiltigt lagerantal. Det måste vara ett icke-negativt heltal.");
            Console.ReadKey(true);
            return;
        }

        var ärUtvald = ConsoleHelper.ReadString("Är produkten utvald? (y/N): ").ToLower() == "y";

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
            ÄrUtvald = ärUtvald
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
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn} {(items[i].ÄrUtvald ? " [UTVALD]" : "")}");
        });
        if (selectedProduct == null) return;

        var product = await _productService.GetByIdAsync(selectedProduct.Id);

        var options = new List<string> { "Namn", "Beskrivning", "Pris", "Lagerantal", "Kategori", "Leverantör", "Färg", "Storlek", "Utvald" };
        var selectedField = MenuBase.NavigateList(options, (items, index) =>
        {
            Console.WriteLine("Vad vill du uppdatera?");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i]}");
        });
        if (selectedField == null) return;

        if (product == null)
        {
            Console.WriteLine("Produkten kunde inte hittas.");
            Console.ReadKey(true);
            return;
        }

        switch (selectedField)
        {
            case "Namn":
                var newName = ConsoleHelper.ReadString("Nytt namn: ");
                if (!ProductValidator.ValidateProductName(newName))
                {
                    Console.WriteLine("Ogiltigt produktnamn. Det måste vara minst 3 tecken långt.");
                    Console.ReadKey(true);
                    return;
                }
                product.Namn = newName;
                break;
            case "Beskrivning":
                var newDescription = ConsoleHelper.ReadString("Ny beskrivning: ");
                if (!ProductValidator.ValidateProductDescription(newDescription))
                {
                    Console.WriteLine("Ogiltig beskrivning. Den måste vara minst 10 tecken lång.");
                    Console.ReadKey(true);
                    return;
                }
                product.Beskrivning = newDescription;
                break;
            case "Pris":
                var newPrice = ConsoleHelper.ReadDecimal("Nytt pris: ");
                if (!ProductValidator.ValidateProductPrice(newPrice))
                {
                    Console.WriteLine("Ogiltigt pris. Det måste vara ett positivt tal.");
                    Console.ReadKey(true);
                    return;
                }
                product.Pris = newPrice;
                break;
            case "Lagerantal":
                var newStock = ConsoleHelper.ReadInt("Nytt lagerantal: ");
                if (!ProductValidator.ValidateProductStock(newStock))
                {
                    Console.WriteLine("Ogiltigt lagerantal. Det måste vara 0 eller ett positivt tal.");
                    Console.ReadKey(true);
                    return;
                }
                product.LagerAntal = newStock;
                break;
            case "Kategori":
                var categories = await _kategoriService.GetAllAsync();
                if (!categories.Any())
                {
                    Console.WriteLine("Inga kategorier finns. Skapa en kategori innan du uppdaterar produkten.");
                    Console.ReadKey(true);
                    return;
                }
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
                if (!leverantörer.Any())
                {
                    Console.WriteLine("Inga leverantörer finns. Skapa en leverantör innan du uppdaterar produkten.");
                    Console.ReadKey(true);
                    return;
                }
                var leverantör = MenuBase.NavigateList(leverantörer.ToList(), (items, index) =>
                {
                    Console.WriteLine("Välj en ny leverantör:");
                    for (int i = 0; i < items.Count; i++)
                        Console.WriteLine($"{(i == index ? "> " : "  ")}{items[i].Namn}");
                });
                if (leverantör != null) product.Leverantör = leverantör;
                break;
            case "Färg":
                var newColor = ConsoleHelper.ReadString("Ny färg: ");
                if (!ProductValidator.ValidateProductColor(newColor))
                {
                    Console.WriteLine("Ogiltig färg.");
                    Console.ReadKey(true);
                    return;
                }
                product.Färg = newColor;
                break;
            case "Storlek":
                var newSize = ConsoleHelper.ReadString("Ny storlek: ");
                if (!ProductValidator.ValidateProductSize(newSize))
                {
                    Console.WriteLine("Ogiltig storlek.");
                    Console.ReadKey(true);
                    return;
                }
                product.Storlek = newSize;
                break;
            case "Utvald":
                product.ÄrUtvald = !product.ÄrUtvald;
                if (product.ÄrUtvald)
                {
                    Console.WriteLine("Produkten är nu utvald!");
                } else
                {
                    Console.WriteLine("Produkten är nu inte längre utvald!");
                }
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
    //Hämtar mest produkter från service
  public async Task<IEnumerable<Produkt>> GetBestProductsAsync(int antal)
    {
        return await _productService.GetBestSellersAsync(antal);
    }

}
