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
    }
}


