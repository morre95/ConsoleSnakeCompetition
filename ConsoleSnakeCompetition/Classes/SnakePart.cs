using System.Drawing;

namespace ConsoleSnakeCompetition.Classes
{
    public class SnakePart
    {
        public char Symbol
        {
            get; set;
        }
        public Point Position
        {
            get; set;
        }

        public SnakePart(Point position, char symbol)
        {
            Position = position;
            Symbol = symbol;
        }

        public void Draw()
        {
            Console.SetCursorPosition(Position.Y, Position.X);
            Console.Write(Symbol);
        }

        public void Erase()
        {
            Console.SetCursorPosition(Position.Y, Position.X);
            Console.Write(' ');
        }
    }
}


