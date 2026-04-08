using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Presentation.UI
{
    public class Box
    {
        public int Width { get; }
        public int Height { get; }
        public int X { get; }
        public int Y { get; }
        public string Title { get; }

        public ConsoleColor BorderColor { get; set; } = Theme.Title;

        public Box(int x, int y, int width, int height, string title)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Title = title;
        }
        public void Draw()
        {
            Console.ForegroundColor = BorderColor;
            // Top border
            Console.SetCursorPosition(X, Y);
            Console.Write("┌" + new string('─', Width - 2) + "┐");
            // Title
            Console.SetCursorPosition(X + 2, Y);
            Console.Write(Title);
            // Side borders
            for (int i = 1; i < Height - 1; i++)
            {
                Console.SetCursorPosition(X, Y + i);
                Console.Write("│");
                Console.SetCursorPosition(X + Width - 1, Y + i);
                Console.Write("│");
            }
            // Bottom border
            Console.SetCursorPosition(X, Y + Height - 1);
            Console.Write("└" + new string('─', Width - 2) + "┘");
            Console.SetCursorPosition(X + 1, Y );
            Console.Write(Title);
            Console.ResetColor();
        }


    }
}
