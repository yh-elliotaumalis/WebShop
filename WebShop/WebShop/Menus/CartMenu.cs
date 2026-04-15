using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;
using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.Validator;

namespace WebShop.Presentation.Menus;

public class CartMenu : MenuBase
{
    private readonly IVarukorgService _varukorgService;
    private readonly IProduktService _produktService;
    private readonly IFraktOmbudRepository _fraktOmbudRepository;
    private readonly IKundService _kundService;
    private readonly CustomerProductHandler _handler;
    private readonly IOrderService _orderService;

    public CartMenu(IProduktService produktService, IVarukorgService varukorgService, IFraktOmbudRepository fraktOmbudRepository, IKundService kundService, IOrderService orderService)
    {
        _produktService = produktService;
        _varukorgService = varukorgService;
        _fraktOmbudRepository = fraktOmbudRepository;
        _kundService = kundService;
        _options = new[] { "Ändra antal", "Tabort produkt", "Rensa varukorg", "Betala", "Tillbaka" };
        _handler = new CustomerProductHandler(produktService);
        _orderService = orderService;
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0: UpdateQuantity(); return false;
            case 1: RemoveProduct(); return false;
            case 2: _varukorgService.Clear(); return false;
            case 3: CheckOut(); return false;
            case 4: return true;
        }
        return false;
    }

    private void ShowCart()
    {
        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        Console.WriteLine("====================================================");
        Console.WriteLine($"{"Namn",-20} {"Antal",6} {"Pris",10}");
        Console.WriteLine("====================================================");
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            Console.WriteLine($"{produkt.Namn,-20} {item.Value + "st",6} {produkt.Pris:0.00} SEK");
            produkter.Add(produkt);
        }
        var totalPris = _varukorgService.CalculateTotal(produkter);
        var moms = totalPris * 0.25m;
        var utanMoms = totalPris - moms;
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"{"Exkl. moms:",-20}  {utanMoms:0.00} SEK");
        Console.WriteLine($"{"Moms (25%):",-20}  {moms:0.00} SEK");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"{"Totalt:",-20}  {totalPris:0.00} SEK");
        Console.WriteLine("====================================================");
    }

    private void RemoveProduct()
    {
        if (_varukorgService.IsEmpty())
        {
            Console.WriteLine("Varukorgen är tom!");
            Console.ReadKey(true);
            return;
        }

        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            produkter.Add(produkt);
        }
        var vald = NavigateList(produkter, (list, i) =>
        {
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j].Namn.PadRight(20)} {list[j].Pris}SEK");
            }
        });
        if (vald == null) return;
        _varukorgService.RemoveProduct(vald.Id);
    }

    private void UpdateQuantity()
    {
        if (_varukorgService.IsEmpty())
        {
            Console.WriteLine("Varukorgen är tom!");
            Console.ReadKey(true);
            return;
        }

        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            produkter.Add(produkt);
        }
        var vald = NavigateList(produkter, (list, i) =>
        {
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j].Namn.PadRight(20)} {list[j].Pris}SEK");
            }
        });
        if (vald == null) return;

        Console.Write("Nytt antal: ");
        var input = Console.ReadLine();
        if (int.TryParse(input, out int nyttAntal) && nyttAntal > 0)
            _varukorgService.UpdateQuantity(vald.Id, nyttAntal);
    }

    private Kund GetCustomerInfo()
    {
        Console.Clear();
        Console.WriteLine("=== Kunduppgifter ===");

        string namn;
        do
        {
            Console.Write("Namn: ");
            namn = Console.ReadLine() ?? "";
        } while (!CustomerValidator.ValidateCustomerName(namn, out _));

        string adress;
        do
        {
            Console.Write("Adress: ");
            adress = Console.ReadLine() ?? "";
        } while (!CustomerValidator.ValidateCustomerAddress(adress, out _));

        string stad;
        do
        {
            Console.Write("Stad: ");
            stad = Console.ReadLine() ?? "";
        } while (!CustomerValidator.ValidateCustomerCity(stad, out _));

        int postnummer;
        do
        {
            Console.Write("Postnummer: ");
            int.TryParse(Console.ReadLine(), out postnummer);
        } while (!CustomerValidator.ValidateCustomerPostNummer(postnummer, out _));

        string mobil;
        do
        {
            Console.Write("Mobilnummer: ");
            mobil = Console.ReadLine() ?? "";
        } while (!CustomerValidator.ValidateCustomerPhone(mobil, out _));

        string epost;
        do
        {
            Console.Write("Epost: ");
            epost = Console.ReadLine() ?? "";
        } while (!CustomerValidator.ValidateCustomerEmail(epost, out _));

        return new Kund
        {
            Id = Guid.NewGuid(),
            Namn = namn,
            Adress = adress,
            Stad = stad,
            Postnummer = postnummer,
            MobilNummer = mobil,
            Epost = epost
        };
    }

    private Kund? GetOrCreateCustomer()
    {
        while (true)
        {
            var alternativ = new List<string> { "Ny kund", "Befintlig kund" };

            var vald = NavigateList(alternativ, (list, i) =>
            {
                Console.Clear();
                Console.WriteLine("=== Kund ===\n");
                for (int j = 0; j < list.Count; j++)
                {
                    var markering = j == i ? "> " : "  ";
                    Console.WriteLine($"{markering}{list[j]}");
                }
            });

            if (vald == null)
                return null;

            if (vald == "Ny kund")
            {
                var nyKund = GetCustomerInfo();
                _kundService.AddAsync(nyKund).GetAwaiter().GetResult();
                return nyKund;
            }

            if (vald == "Befintlig kund")
            {
                Console.Clear();
                Console.Write("Ange ditt mobilnummer: ");
                var telefon = Console.ReadLine() ?? "";
                if (!CustomerValidator.ValidateCustomerPhone(telefon, out var err))
                {
                    Console.WriteLine(err);
                    Console.ReadKey(true);
                    continue;
                }

                var allaKunder = _kundService.GetAllAsync().GetAwaiter().GetResult();
                var kund = allaKunder.FirstOrDefault(k => k.MobilNummer == telefon);

                if (kund == null)
                {
                    Console.WriteLine("Ingen kund hittades med det numret. Tryck valfri tangent.");
                    Console.ReadKey(true);
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"Hittade: {kund.Namn} – {kund.Adress}, {kund.Stad}");
                Console.WriteLine("Tryck Enter för att fortsätta eller Escape för att gå tillbaka.");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter)
                    return kund;
            }
        }
    }
    private FraktOmbud GetShippingMetod()
    {
        var alternativ = _fraktOmbudRepository.GetAllAsync().GetAwaiter().GetResult().ToList();

        var vald = NavigateList(alternativ, (list, i) =>
        {
            Console.WriteLine("=== Välj fraktmetod ===\n");
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j].Namn} - {list[j].Pris:0.00}kr");
            }
        });

        return vald!;
    }

    private Betalsätt GetPaymentMetod()
    {
        var alternativ = Enum.GetNames<Betalsätt>().ToList();

        var vald = NavigateList(alternativ, (list, i) =>
        {
            Console.WriteLine("=== Välj betalmetod ===\n");
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j]}");
            }
        });

        return Enum.Parse<Betalsätt>(vald!);
    }

    private void ShowConfirmation(Kund kund, FraktOmbud frakt, Betalsätt betalsätt)
    {
        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            produkter.Add(produkt);
        }
        var totalPris = _varukorgService.CalculateTotal(produkter) + frakt.Pris;
        var moms = totalPris * 0.25m;
        var utanMoms = totalPris - moms;

        Console.Clear();
        Console.WriteLine("=== Orderbekräftelse ===");
        Console.WriteLine($"Kund:         {kund.Namn}");
        Console.WriteLine($"Adress:       {kund.Adress}, {kund.Postnummer} {kund.Stad}");
        Console.WriteLine($"Frakt:        {frakt.Namn} {frakt.Pris:0.00}SEK");
        Console.WriteLine($"Betalsätt:    {betalsätt}");
        Console.WriteLine("==========================");
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            Console.WriteLine($"{produkt.Namn.PadRight(20)} {item.Value}st {produkt.Pris:0.00}SEK");
        }
        Console.WriteLine("------------------------");
        Console.WriteLine($"Exkl. moms:   {utanMoms:0.00}SEK");
        Console.WriteLine($"Moms (25%):   {moms:0.00}SEK");
        Console.WriteLine($"Frakt:        {frakt.Pris:0.00}SEK");
        Console.WriteLine("------------------------");
        Console.WriteLine($"Totalt:       {totalPris:0.00}SEK");
        Console.WriteLine("==========================");
        Console.WriteLine("\nTack för din beställning!");

        var produktOrdrar = items.Select(item =>
        {
            var produkt = produkter.First(p => p.Id == item.Key);
            return new ProduktOrder
            {
                Id = Guid.NewGuid(),
                ProduktId = item.Key,
                Antal = item.Value,
                PrisvidKöp = produkt.Pris
            };
        }).ToList();

        _orderService.CreateOrderAsync(kund.Id, produktOrdrar, frakt.Id, betalsätt).GetAwaiter().GetResult();

        _varukorgService.Clear();

        Console.ReadKey(true);
    }

    private void CheckOut()
    {
        var kund = GetOrCreateCustomer();
        if (kund == null) return;

        var frakt = GetShippingMetod();
        var betalsätt = GetPaymentMetod();
        ShowConfirmation(kund, frakt, betalsätt);
    }

    protected override void DrawContent()
    {
        Console.Clear();
        ShowCart();
    }
}