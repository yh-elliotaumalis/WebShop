using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using WebShop.Presentation.Menus;
using WebShop.Presentation.Validator;

namespace WebShop.Presentation.MenuHandlers;

public class AdminCustomerHandler
{
    private readonly IKundService _kundService;

    public AdminCustomerHandler(IKundService kundService)
    {
        _kundService = kundService;
    }

    async Task<Kund?> SelectCustomer()
    {
        var searchTerm = ConsoleHelper.ReadString("Sök kunder (tryck Enter för att visa alla): ");
        IEnumerable<Kund> customers;
        if (string.IsNullOrEmpty(searchTerm))
        {
            customers = await _kundService.GetAllAsync();
        }
        else
        {
            customers = await _kundService.GetBySearchAsync(searchTerm);
        }

        if (!customers.Any())
        {
            Console.WriteLine("Inga kunder hittades.");
            Console.ReadKey(true);
            return null;
        }
        var selectedCustomer = MenuBase.NavigateList(customers.ToList(), (list, selectedIndex) =>
        {
            Console.WriteLine("Kunder:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn} - {list[i].Epost}");
            }
        });

        if (selectedCustomer == null)
        {
            return null;
        }

        var customer = await _kundService.GetByIdAsync(selectedCustomer.Id);
        if (customer == null)
        {
            Console.WriteLine("Kunden kunde inte hittas.");
            return null;
        }

        return customer;
    }

    public async Task HandleShowCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Visa kunder ===\n");

        var customer = await SelectCustomer();

        if (customer == null)
        {
            return;
        }

        Console.Clear();
        Console.WriteLine($"=== Kund: {customer.Namn} ===\n");
        Console.WriteLine($"E-post: {customer.Epost}");
        Console.WriteLine($"Mobilnummer: {customer.MobilNummer}");
        Console.WriteLine($"Stad: {customer.Stad}");
        Console.WriteLine($"Adress: {customer.Adress}");
        Console.WriteLine($"Postnummer: {customer.Postnummer}");

        Console.WriteLine($"\n=== Orderhistorik ===\n");

        if (customer.Ordrar.Any())
        {
            customer.Ordrar.ForEach(order =>
            {
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Datum: {order.OrderDatum}");
                Console.WriteLine($"Totalpris: {order.TotalPris} kr");
                Console.WriteLine($"Status: {(order.ÄrBetald ? "Betald" : "Obetald")}");
                Console.WriteLine($"Betalsätt: {order.Betalsätt}");
                Console.WriteLine($"Fraktombud: {order.FraktOmbud.Namn}");
                Console.WriteLine("Produkter:");
                order.ProduktOrdrar.ForEach(productOrder =>
                {
                    var produkt = productOrder.Produkt;
                    Console.WriteLine($"- {produkt.Namn} ({productOrder.PrisvidKöp})");
                });
                Console.WriteLine();
            });
        }
        else
        {
            Console.WriteLine("Denna kund har inga ordrar.");
        }
        Console.ReadKey(true);
    }

    public async Task HandleAddCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till kund ===\n");

        var namn = ConsoleHelper.ReadString("Kundens namn: ");
        if (!CustomerValidator.ValidateCustomerName(namn, out var nameError))
        {
            Console.WriteLine(nameError);
            Console.ReadKey(true);
            return;
        }

        var email = ConsoleHelper.ReadString("Kundens e-post: ");
        if (!CustomerValidator.ValidateCustomerEmail(email, out var emailError))
        {
            Console.WriteLine(emailError);
            Console.ReadKey(true);
            return;
        }

        var phoneNumber = ConsoleHelper.ReadString("Kundens mobilnummer: ");
        if (!CustomerValidator.ValidateCustomerPhone(phoneNumber, out var phoneError))
        {
            Console.WriteLine(phoneError);
            Console.ReadKey(true);
            return;
        }

        var city = ConsoleHelper.ReadString("Kundens stad: ");
        if (!CustomerValidator.ValidateCustomerCity(city, out var cityError))
        {
            Console.WriteLine(cityError);
            Console.ReadKey(true);
            return;
        }

        var address = ConsoleHelper.ReadString("Kundens adress: ");
        if (!CustomerValidator.ValidateCustomerAddress(address, out var addressError))
        {
            Console.WriteLine(addressError);
            Console.ReadKey(true);
            return;
        }

        var postNummer = ConsoleHelper.ReadInt("Kundens postnummer: ");
        if (!CustomerValidator.ValidateCustomerPostNummer(postNummer, out var postNummerError))
        {
            Console.WriteLine(postNummerError);
            Console.ReadKey(true);
            return;
        }

        var customer = new Kund
        {
            Id = Guid.NewGuid(),
            Namn = namn,
            Epost = email,
            MobilNummer = phoneNumber,
            Postnummer = postNummer,
            Stad = city,
            Adress = address
        };

        await _kundService.AddAsync(customer);

        Console.WriteLine($"Kunden '{namn}' har skapats.");
        Console.ReadKey(true);
    }

    public async Task HandleUpdateCustomerAsync()
    {
        Console.WriteLine("=== Uppdatera kund ===\n");

        var customer = await SelectCustomer();

        if (customer == null)
        {
            return;
        }

        var options = new List<string> { "Namn", "E-post", "Mobilnummer", "Stad", "Adress", "Postnummer" };
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
                if (!CustomerValidator.ValidateCustomerName(newName, out var nameError))
                {
                    Console.WriteLine(nameError);
                    Console.ReadKey(true);
                    return;
                }
                customer.Namn = newName;
                break;
            case "E-post":
                var newEmail = ConsoleHelper.ReadString("Ny e-post: ");
                if (!CustomerValidator.ValidateCustomerEmail(newEmail, out var emailError))
                {
                    Console.WriteLine(emailError);
                    Console.ReadKey(true);
                    return;
                }
                customer.Epost = newEmail;
                break;
            case "Mobilnummer":
                var newPhoneNumber = ConsoleHelper.ReadString("Nytt mobilnummer: ");
                if (!CustomerValidator.ValidateCustomerPhone(newPhoneNumber, out var phoneNumberError))
                {
                    Console.WriteLine(phoneNumberError);
                    Console.ReadKey(true);
                    return;
                }
                customer.MobilNummer = newPhoneNumber;
                break;
            case "Stad":
                var newCity = ConsoleHelper.ReadString("Ny stad: ");
                if (!CustomerValidator.ValidateCustomerCity(newCity, out var cityError))
                {
                    Console.WriteLine(cityError);
                    Console.ReadKey(true);
                    return;
                }
                customer.Stad = newCity;
                break;
            case "Adress":
                var newAddress = ConsoleHelper.ReadString("Ny adress: ");
                if (!CustomerValidator.ValidateCustomerAddress(newAddress, out var addressError))
                {
                    Console.WriteLine(addressError);
                    Console.ReadKey(true);
                    return;
                }
                customer.Adress = newAddress;
                break;
            case "Postnummer":
                var newPostnummer = ConsoleHelper.ReadInt("Nytt postnummer: ");
                if (!CustomerValidator.ValidateCustomerPostNummer(newPostnummer, out var postalCodeError))
                {
                    Console.WriteLine(postalCodeError);
                    Console.ReadKey(true);
                    return;
                }
                customer.Postnummer = newPostnummer;
                break;
        }

        await _kundService.UpdateAsync(customer);

        Console.WriteLine("Kunden har uppdaterats.");
        Console.ReadKey(true);
    }
    public async Task HandleDeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ta bort kund ===\n");

        var customer = await SelectCustomer();
        if (customer == null)
        {
            return;
        }

        Console.WriteLine("Är du säker på att du vill ta bort kunden? Detta kan inte ångras! (y/n)");
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Y)
        {
            try
            {
                await _kundService.DeleteAsync(customer.Id);
                Console.WriteLine("Kunden har tagits bort.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte ta bort kunden: {ex.Message}");
            }
            Console.ReadKey(true);
        }

        return;
    }
}
