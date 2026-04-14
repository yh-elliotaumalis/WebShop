using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Presentation.Validator;

public class CategoryValidator
{
    public static bool ValidateCategoryName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        if (name.Length < 3)
        {
            return false;
        }
        return true;
    }
}
