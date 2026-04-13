namespace HustlersAB.Admin.Menus;

public abstract class MenuBase
{
    protected string[] _options = Array.Empty<string>();

 
    protected bool _isProductFocused = true;

    protected virtual void PopuleraProduker() { }

   
    protected virtual void MoveLeft() { }
    protected virtual void MoveRight() { }
    protected virtual void HandleProductEnter() { }

    protected void PrintMenu(int selectedIndex, string title)
    {
        WriteCentered($"=== {title} ===");
        Console.WriteLine();

        int maxLength = _options.Max(o => o.Length);

        for (int i = 0; i < _options.Length; i++)
        {
            string prefix = (i == selectedIndex) ? "> " : "  ";
            string paddedText = _options[i].PadRight(maxLength);

            WriteCentered($"{prefix}{paddedText}");
        }
    }

    public void ShowMenu(string title)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();

            ShowWelcome();

            PopuleraProduker();

            Console.WriteLine();
            Console.WriteLine();

            PrintMenu(selectedIndex, title);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Tab:
                    _isProductFocused = !_isProductFocused;
                    break;

                case ConsoleKey.LeftArrow:
                    if (_isProductFocused)
                        MoveLeft();
                    break;

                case ConsoleKey.RightArrow:
                    if (_isProductFocused)
                        MoveRight();
                    break;

                case ConsoleKey.UpArrow:
                    if (!_isProductFocused)
                        selectedIndex = (selectedIndex - 1 + _options.Length) % _options.Length;
                    break;

                case ConsoleKey.DownArrow:
                    if (!_isProductFocused)
                        selectedIndex = (selectedIndex + 1) % _options.Length;
                    break;

                case ConsoleKey.Enter:
                    if (_isProductFocused)
                    {
                        HandleProductEnter();
                    }
                    else
                    {
                        if (ExecuteChoice(selectedIndex))
                            return;
                    }
                    break;
            }
        }
    }

    protected void ShowWelcome()
    {
        WriteCentered("Välkommen till KläderShoppen!");
        WriteCentered("********************************");
        Console.WriteLine();

        WriteCentered("Här kan du bläddra bland produkter och handla enkelt.");
        WriteCentered("Använd piltangenterna för att navigera och Enter för att välja.");
    }

    protected void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;

        Console.WriteLine(new string(' ', Math.Max(0, spaces)) + text);
    }

    protected abstract bool ExecuteChoice(int selectedIndex);
}