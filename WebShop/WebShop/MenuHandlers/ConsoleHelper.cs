namespace WebShop.Presentation.MenuHandlers;

public static class ConsoleHelper
{
    public static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
                return result;
            Console.WriteLine("Ogiltigt tal. Försök igen.");
        }
    }

    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal result))
                return result;
            Console.WriteLine("Ogiltigt tal. Försök igen.");
        }
    }
}
