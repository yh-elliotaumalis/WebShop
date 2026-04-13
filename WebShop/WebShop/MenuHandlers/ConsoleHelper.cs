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

    public static int OptionPicker(string prompt, List<string> options)
    {
        Console.WriteLine(prompt);

        var selectedIndex = 0;
        var cursorPosition = Console.GetCursorPosition();

        while (true)
        {
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            for (int i = 0; i < options.Count; i++)
            {
                var prefix = (i == selectedIndex) ? "> " : "  ";
                Console.WriteLine($"{prefix}{options[i]}");
            }

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(options.Count - 1, selectedIndex + 1);
                    break;

                case ConsoleKey.Enter:
                    return selectedIndex;
            }
        }
    }
}
