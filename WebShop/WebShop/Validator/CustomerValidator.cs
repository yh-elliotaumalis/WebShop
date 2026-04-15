using System.Text.RegularExpressions;

namespace WebShop.Presentation.Validator;

public class CustomerValidator
{
    public static bool ValidateCustomerName(string name, out string? error)
    {
        if (string.IsNullOrWhiteSpace(name)) { error = "Namnet får inte vara tomt."; return false; }
        if (name.Length < 3) { error = "Namnet måste vara minst 3 tecken långt."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerAddress(string address, out string? error)
    {
        if (string.IsNullOrWhiteSpace(address)) { error = "Adressen får inte vara tom."; return false; }
        if (address.Length < 3) { error = "Adressen måste vara minst 3 tecken lång."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerCity(string city, out string? error)
    {
        if (string.IsNullOrWhiteSpace(city)) { error = "Staden får inte vara tom."; return false; }
        if (city.Length < 2) { error = "Staden måste vara minst 2 tecken lång."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerPostNummer(int postNummer, out string? error)
    {
        if (postNummer < 10000 || postNummer > 99999) { error = "Postnumret måste vara ett femsiffrigt nummer."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerPhone(int phone, out string? error)
    {
        if (phone < 1000000) { error = "Mobilnumret måste vara minst 7 siffror."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerEmail(string email, out string? error)
    {
        if (string.IsNullOrWhiteSpace(email)) { error = "E-postadressen får inte vara tom."; return false; }
        var regex = new Regex("^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$");
        if (!regex.IsMatch(email)) { error = "E-postadressen är inte i ett giltigt format."; return false; }
        error = null;
        return true;
    }

    public static string GetValidatedName()
    {
        string input;
        do
        {
            Console.Write("Namn: ");
            input = Console.ReadLine() ?? "";
            if (!ValidateCustomerName(input, out var err)) Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }

    public static string GetValidatedAddress()
    {
        string input;
        do
        {
            Console.Write("Adress: ");
            input = Console.ReadLine() ?? "";
            if (!ValidateCustomerAddress(input, out var err)) Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }

    public static string GetValidatedCity()
    {
        string input;
        do
        {
            Console.Write("Stad: ");
            input = Console.ReadLine() ?? "";
            if (!ValidateCustomerCity(input, out var err)) Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }

    public static int GetValidatedPostNummer()
    {
        int input;
        do
        {
            Console.Write("Postnummer: ");
            int.TryParse(Console.ReadLine(), out input);
            if (!ValidateCustomerPostNummer(input, out var err)) Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }

    public static int GetValidatedPhone()
    {
        int input;
        do
        {
            Console.Write("Mobilnummer: ");
            if (!int.TryParse(Console.ReadLine(), out input))
                Console.WriteLine("Mobilnumret måste vara ett nummer.");
            else if (!ValidateCustomerPhone(input, out var err))
                Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }

    public static string GetValidatedEmail()
    {
        string input;
        do
        {
            Console.Write("Epost: ");
            input = Console.ReadLine() ?? "";
            if (!ValidateCustomerEmail(input, out var err)) Console.WriteLine(err);
            else break;
        } while (true);
        return input;
    }
}