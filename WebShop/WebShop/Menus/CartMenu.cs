using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class CartMenu : MenuBase
{
    private readonly IVarukorgService _varukorgService;
    private readonly IProduktService _produktService;
    private readonly CustomerProductHandler _handler;

    public CartMenu(IProduktService produktService, IVarukorgService varukorgService)
    {
        _produktService = produktService;
        _varukorgService = varukorgService;
        _options = new[] { "Ändra antal", "Tabort produkt", "Rensa varukorg", "Betala", "Tillbaka" };
        _handler = new CustomerProductHandler(produktService);
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                UpdateQuantity();
                return false;
            case 1:
                RemoveProduct();
                return false;
            case 2:
                _varukorgService.Clear();
                return false;
            case 3:
                CheckOut();
                return false;
            case 4:
                return true;
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
        Console.Write("Namn: ");
        var namn = Console.ReadLine() ?? "";
        Console.Write("Adress: ");
        var adress = Console.ReadLine() ?? "";
        Console.Write("Stad: ");
        var stad = Console.ReadLine() ?? "";
        Console.Write("Postnummer: ");
        int.TryParse(Console.ReadLine(), out int postnummer);
        Console.Write("Mobilnummer: ");
        int.TryParse(Console.ReadLine(), out int mobil);
        Console.Write("Epost: ");
        var epost = Console.ReadLine() ?? "";

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
    private (string namn, decimal pris, string leveranstid) GetShippingMetod()
    {
        var alternativ = new List<string>
    {
        "PostNord - 49kr (3-5 dagar)",
        "DHL - 99kr (1-3 dagar)"
    };

        var vald = NavigateList(alternativ, (list, i) =>
        {
            Console.WriteLine("=== Välj fraktmetod ===\n");
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j]}");
            }
        });

        return vald == alternativ[0] ? ("PostNord", 49m, "3-5 dagar") : ("DHL", 99m, "1-3 dagar");
    }


    private Betalsätt GetPaymentMetod()
    {
        var alternativ = new List<string> { "Kort", "Swish", "Faktura" };

        var vald = NavigateList(alternativ, (list, i) =>
        {
            Console.WriteLine("=== Välj betalmetod ===\n");
            for (int j = 0; j < list.Count; j++)
            {
                var markering = j == i ? "> " : "  ";
                Console.WriteLine($"{markering}{list[j]}");
            }
        });

        return vald switch
        {
            "Kort" => Betalsätt.Kort,
            "Swish" => Betalsätt.Swish,
            _ => Betalsätt.Faktura
        };
    }



    private void ShowConfirmation(Kund kund, string fraktNamn, decimal fraktPris, string leveranstid, Betalsätt betalsätt)
    {
        var items = _varukorgService.GetItems();
        var produkter = new List<Produkt>();
        foreach (var item in items)
        {
            var produkt = _handler.GetProductAsync(item.Key).GetAwaiter().GetResult()!;
            produkter.Add(produkt);
        }
        var totalPris = _varukorgService.CalculateTotal(produkter) + fraktPris;
        var moms = totalPris * 0.25m;
        var utanMoms = totalPris - moms;

        Console.Clear();
        Console.WriteLine("=== Orderbekräftelse ===");
        Console.WriteLine($"Kund:         {kund.Namn}");
        Console.WriteLine($"Adress:       {kund.Adress}, {kund.Postnummer} {kund.Stad}");
        Console.WriteLine($"Frakt:        {fraktNamn} {fraktPris:0.00}SEK ({leveranstid})");
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
        Console.WriteLine($"Frakt:        {fraktPris:0.00}SEK");
        Console.WriteLine("------------------------");
        Console.WriteLine($"Totalt:       {totalPris:0.00}SEK");
        Console.WriteLine("==========================");
        Console.WriteLine("\nTack för din beställning!");

        _varukorgService.Clear();
        Console.ReadKey(true);
    }
    private void CheckOut()
    {
        var kund = GetCustomerInfo();
        var (fraktNamn, fraktPris, leveranstid) = GetShippingMetod();
        var betalsätt = GetPaymentMetod();
        ShowConfirmation(kund, fraktNamn, fraktPris, leveranstid, betalsätt);
    }

    protected override void DrawContent()
    {
        Console.Clear();
        ShowCart();
    }
}
