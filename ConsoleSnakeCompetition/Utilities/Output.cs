namespace ConsoleSnakeCompetition.Utilities
{
    public static class Output
    {
        public static void WriteLine(ConsoleColor color, object? value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string? value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void Write(ConsoleColor color, object? value)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void Write(ConsoleColor color, string? value)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void Write(ConsoleColor color, char value)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        static void WriteOnBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Write(Console.ForegroundColor, text);
            Console.SetCursorPosition(x, y);
        }

        static void WriteOnBottomLine(ConsoleColor color, string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Write(color, text);
            Console.SetCursorPosition(x, y);
        }

    }
}


