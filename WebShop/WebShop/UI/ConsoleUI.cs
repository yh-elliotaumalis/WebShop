using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Presentation.UI
{
    public class ConsoleUI
    {
        public static void Highlight(string text, bool selected)
        {
            if (selected)
            {
                Console.BackgroundColor = Theme.MenuSelectedBg;
                Console.ForegroundColor = Theme.MenuSelectedText;
            }
            else
            {
                Console.ForegroundColor= Theme.DefaultText;
            }
            Console.Write(text.PadRight(20));
            Console.ResetColor();
        }
        public static void WritePrice(decimal price)
        {
            Console.ForegroundColor = Theme.Price;
            Console.Write($"price: {price} Kr");
            Console.ResetColor();
        }
        public static void WriteHint(string text)
        {
            Console.ForegroundColor = Theme.Hint;
            Console.Write(text);
            Console.ResetColor();
        }
        public static void Message(string msg)
        {
            Console.SetCursorPosition(2, Console.WindowHeight - 2);
            Console.ForegroundColor = Theme.Message;
            Console.Write(msg.PadRight(Console.WindowWidth-4));
            Console.ResetColor();
        }
        public static void Footer(string Text="↑↓ Navigera | Enter = välj | A/B/C = köp | K = checkout | ESC = tillbaka")
        {
            Console.SetCursorPosition(2, Console.WindowHeight - 1);
            Console.ForegroundColor = Theme.Hint;
            Console.Write(Text.PadRight(Console.WindowWidth - 4));
            Console.ResetColor();
        }
    }
}
