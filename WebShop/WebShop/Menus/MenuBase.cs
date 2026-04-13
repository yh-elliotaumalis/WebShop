namespace HustlersAB.Admin.Menus;

public abstract class MenuBase
{
    protected string[] _options = Array.Empty<string>();

    protected void PrintMenu(int selectedIndex, string title)
    {
        WriteCentered($"=== {title} ===");
        Console.WriteLine();

        int maxLength = _options.Max(o => o.Length);

        for (int i = 0; i < _options.Length; i++)
        {
            string prefix = (i == selectedIndex) ? "> " : "  ";


            string paddedText = _options[i].PadRight(maxLength);


            string fullText = prefix + paddedText;

            WriteCentered(fullText);
        }
    }

    public void ShowMenu(string title)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();


            ShowWelcome();

            Console.WriteLine();
            Console.WriteLine();

            PrintMenu(selectedIndex, title);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + _options.Length) % _options.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % _options.Length;
                    break;

                case ConsoleKey.Enter:
                    if (ExecuteChoice(selectedIndex))
                        return;
                    break;
            }
        }
    }

    private void ShowWelcome()
    {
        WriteCentered("Välkommen till KläderShoppen!");
        WriteCentered("********************************");
        Console.WriteLine();

        WriteCentered("Här kan du bläddra bland produkter och handla enkelt.");
        WriteCentered("Använd piltangenterna för att navigera och Enter för att välja.");
    }

    private void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;

        Console.WriteLine(new string(' ', Math.Max(0, spaces)) + text);
    }
    public static T? NavigateList<T>(List<T> items, Action<List<T>, int> draw) where T : class
    {
        int selectedIndex = 0;
        while (true)
        {
            Console.Clear();
            draw(items, selectedIndex);
            switch (Console.ReadKey(true).Key)
            {

                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(items.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    return items[selectedIndex];
                case ConsoleKey.Escape:
                    return null;
            }
        }
    }
    protected abstract bool ExecuteChoice(int selectedIndex);
}