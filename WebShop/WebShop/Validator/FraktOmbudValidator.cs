using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebShop.Presentation.Validator;

public class FraktOmbudValidator
{
    public static bool ValidateFraktOmbudName(string name, out string? error)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            error = "Namnet får inte vara tomt.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateFraktOmbudPrice(decimal price, out string? error)
    {
        if (price < 0)
        {
            error = "Priset måste vara större än noll.";
            return false;
        }
        error = null;
        return true;
    }
}
