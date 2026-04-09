namespace WebShop.Presentation.Validator;

public class SearchValidator
{
    public string? Validate(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        return "Sökning får inte vara tom";
    }
}
