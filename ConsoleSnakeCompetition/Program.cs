using System.Drawing;

namespace ConsoleSnakeCompetition;

internal class Program
{
    static void Main(string[] args)
    {
        int rows = 20, cols = 80;
        string toPrint = "";
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (row == 0 || row > rows - 2 || col == 0 || col > cols - 2)
                {
                    toPrint += "*";
                }
                else
                {
                    toPrint += " ";
                }
            }
            toPrint += "\n";
        }

        Console.WriteLine(toPrint);

        Snake snake = new Snake(new Point(rows / 2, cols / 2), 5);
        snake.Draw();
        while (true) 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                snake.Move(Snake.Direction.Up);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                snake.Move(Snake.Direction.Down);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                snake.Move(Snake.Direction.Left);
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                snake.Move(Snake.Direction.Right);
            }
        }
    }

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

    public class Snake
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }

        public char Symbol
        {
            get; set;
        }

        public Direction CurrentDirection
        {
            get; set;
        }

        private SnakePart[] _body;
        private SnakePart Head => _body.First();
        private SnakePart Tail => _body.Last();

        public Snake(Point startingPoint, int length, char symbol = '#')
        {
            Symbol = symbol;

            _body = Enumerable
                .Range(0, length)
                .Select(x => new SnakePart(startingPoint, symbol))
                .ToArray();
        }

        public void Draw()
        {
            foreach (var snakePart in _body)
            {
                snakePart.Draw();
            }
        }

        public void Move(Direction direction)
        {
            SnakePart newHead = null;

            switch (direction)
            {
                case Direction.Up:
                    newHead = new SnakePart(new Point(Head.Position.X - 1, Head.Position.Y), Symbol);
                    CurrentDirection = Direction.Up;
                    break;
                case Direction.Down:
                    newHead = new SnakePart(new Point(Head.Position.X + 1, Head.Position.Y), Symbol);
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Left:
                    newHead = new SnakePart(new Point(Head.Position.X, Head.Position.Y - 1), Symbol);
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Right:
                    newHead = new SnakePart(new Point(Head.Position.X, Head.Position.Y + 1), Symbol);
                    CurrentDirection = Direction.Right;
                    break;
            }

            Tail.Erase();
            for (var i = _body.Length - 1; i > 0; i--)
            {
                _body[i] = _body[i - 1];
            }
            _body[0] = newHead;
            Draw();
        }
    }
}
