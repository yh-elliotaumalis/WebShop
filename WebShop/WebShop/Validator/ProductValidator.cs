using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Presentation.Validator;

public class ProductValidator
{
    public static bool ValidateProductName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        if (name.Length < 3)
        {
            return false;
        }

        return true;
    }

    public static bool ValidateProductPrice(decimal price)
    {
        return price > 0;
    }

    public static bool ValidateProductStock(int stock)
    {
        return stock >= 0;
    }

    public static bool ValidateProductSize(string size)
    {
        return !string.IsNullOrEmpty(size);
    }

    public static bool ValidateProductColor(string color)
    {
        return !string.IsNullOrEmpty(color);
    }
}
