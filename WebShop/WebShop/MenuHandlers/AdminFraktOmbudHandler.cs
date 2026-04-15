using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using Webshop.Domain.Entitites;
using WebShop.Presentation.Validator;

namespace WebShop.Presentation.MenuHandlers;

public class AdminFraktOmbudHandler
{
    private readonly IFraktOmbudService _fraktOmbudService;

    public AdminFraktOmbudHandler(IFraktOmbudService fraktOmbudService)
    {
        _fraktOmbudService = fraktOmbudService;
    }

    public async Task HandleAddFraktOmbudAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till fraktombud ===\n");

        var name = ConsoleHelper.ReadString("Ange fraktombudets namn: ");
        if (!FraktOmbudValidator.ValidateFraktOmbudName(name, out var namnError)) { 
            Console.WriteLine(namnError);
            Console.ReadKey(true);
            return;
        }

        var pris = ConsoleHelper.ReadDecimal("Ange fraktombudets pris: ");
        if (!FraktOmbudValidator.ValidateFraktOmbudPrice(pris, out var prisError)) { 
            Console.WriteLine(prisError);
            Console.ReadKey(true);
            return;
        }

        var fraktOmbud = new FraktOmbud {
            Id = Guid.NewGuid(),
            Namn = name,
            Pris = pris,
        };

        await _fraktOmbudService.AddAsync(fraktOmbud);

        Console.WriteLine($"Fraktombudet '{name}' har skapats.");
        Console.ReadKey(true);
    }

    public async Task HandleUpdateFraktOmbudAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Uppdatera fraktombud ===\n");

        var allaFraktOmbud = await _fraktOmbudService.GetAllAsync();

        var fraktOmbud = MenuBase.NavigateList(allaFraktOmbud.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj ett fraktombud att uppdatera:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (fraktOmbud == null)
        {
            Console.WriteLine("Inget fraktombud valt.");
            Console.ReadKey(true);
            return;
        }

        var options = new List<string> { "Namn", "Pris" };
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
                var newName = ConsoleHelper.ReadString("Nytt namn: ");
                if (!FraktOmbudValidator.ValidateFraktOmbudName(newName, out var namnError))
                {
                    Console.WriteLine(namnError);
                    Console.ReadKey(true);
                    return;
                }
                fraktOmbud.Namn = newName;
                break;
            case "Pris":
                var newPrice = ConsoleHelper.ReadDecimal("Nytt pris: ");
                if (!FraktOmbudValidator.ValidateFraktOmbudPrice(newPrice, out var prisError))
                {
                    Console.WriteLine(prisError);
                    Console.ReadKey(true);
                    return;
                }
                fraktOmbud.Pris = newPrice;
                break;
        }

        await _fraktOmbudService.UpdateAsync(fraktOmbud);

        Console.WriteLine($"Fraktombudet '{fraktOmbud.Namn}' har uppdaterats.");
        Console.ReadKey(true);
    }

    public async Task HandleDeleteFraktOmbudAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ta bort fraktombud ===\n");

        var allaFraktOmbud = await _fraktOmbudService.GetAllAsync();

        var fraktOmbud = MenuBase.NavigateList(allaFraktOmbud.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj ett fraktombud att ta bort:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (fraktOmbud == null)
        {
            Console.WriteLine("Inget fraktombud valt.");
            Console.ReadKey(true);
            return;
        }

        if (fraktOmbud.Ordrar.Any())
        {
            Console.WriteLine("Det går inte att ta bort ett fraktombud som har ordrar kopplade.");
            Console.ReadKey(true);
            return;
        }

        await _fraktOmbudService.DeleteAsync(fraktOmbud.Id);
        Console.WriteLine($"Fraktombudet '{fraktOmbud.Namn}' har tagits bort.");
        Console.ReadKey(true);
    }
}
