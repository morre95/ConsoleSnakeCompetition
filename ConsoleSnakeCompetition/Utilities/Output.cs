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

        public static void WriteAt(ConsoleColor color, object? value, int left, int top)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(left, top);
            Console.Write(value);
            Console.ResetColor();
            Console.SetCursorPosition(x, y);
        }

        public static void WriteOnBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.SetCursorPosition(0, Console.WindowTop + Console.WindowHeight - 1);
            Write(Console.ForegroundColor, text);
            Console.SetCursorPosition(x, y);
        }

        public static void WriteOnBottomLine(ConsoleColor color, string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.SetCursorPosition(0, Console.WindowTop + Console.WindowHeight - 1);
            Write(color, text);
            Console.SetCursorPosition(x, y);
        }


        public static string ReadLine(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        public static string ReadLine(ConsoleColor color, string text)
        {
            Write(color, text);
            return Console.ReadLine();
        }

    }
}


