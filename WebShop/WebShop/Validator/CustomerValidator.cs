using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebShop.Presentation.Validator;

public class CustomerValidator
{
    public static bool ValidateCustomerName(string name, out string? error)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            error = "Namnet får inte vara tomt.";
            return false;
        }
        if (name.Length < 3)
        {
            error = "Namnet måste vara minst 3 tecken långt.";
            return false;
        }

    public static bool ValidateCustomerAddress(string address, out string? error)
    {
        if (string.IsNullOrWhiteSpace(address)) { error = "Adressen får inte vara tom."; return false; }
        if (address.Length < 3) { error = "Adressen måste vara minst 3 tecken lång."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerEmail(string email, out string? error)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            error = "E-postadressen får inte vara tom.";
            return false;
        }

        if (email.Length < 3)
        {
            error = "E-postadressen måste vara minst 3 tecken lång.";
            return false;
        }

        // Regex för att validera e-postadresser. https://regex101.com/r/SOgUIV/2
        var regex = new Regex("^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$");
        if (!regex.IsMatch(email))
        {
            error = "E-postadressen är inte i ett giltigt format.";
            return false;
        }

    public static bool ValidateCustomerEmail(string email, out string? error)
    {
        if (string.IsNullOrWhiteSpace(email)) { error = "E-postadressen får inte vara tom."; return false; }
        var regex = new Regex("^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$");
        if (!regex.IsMatch(email)) { error = "E-postadressen är inte i ett giltigt format."; return false; }
        error = null;
        return true;
    }

    public static bool ValidateCustomerPhone(string phone, out string? error)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            error = "Mobilnumret får inte vara tomt.";
        }

        if (phone.Length < 7)
        {
            error = "Mobilnumret måste vara minst 7 tecken långt.";
            return false;
        }

        // Regex för att validera telefonnummer med olika format. https://regex101.com/r/j48BZs/2
        var regex = new Regex("^(\\+\\d{1,2}\\s?)?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$");
        if (!regex.IsMatch(phone))
        {
            error = "Mobilnumret är inte i ett giltigt format.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateCustomerAddress(string address, out string? error)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            error = "Adressen får inte vara tom.";
            return false;
        }
        if (address.Length < 3)
        {
            error = "Adressen måste vara minst 3 tecken lång.";
            return false;
        }
        error = null;
        return true;
    }

    public static bool ValidateCustomerPostNummer(int postNummer, out string? error)
    {
        if (postNummer < 10000 || postNummer > 99999)
        {
            error = "Postnumret måste vara ett femsiffrigt nummer.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateCustomerCity(string city, out string? error)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            error = "Staden får inte vara tom.";
            return false;
        }
        if (city.Length < 2)
        {
            error = "Staden måste vara minst 2 tecken lång.";
            return false;
        }
        error = null;
        return true;
    }
}
