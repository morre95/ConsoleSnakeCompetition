using System.Drawing;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int rows = 20, cols = 80;
            Grid<char> grid = PopulateEmptyGrid(rows, cols);

            grid.SetValue(18, 78, '%');

            string toPrint = "";
            for (int row = 0; row < grid.RowCount(); row++)
            {
                for (int col = 0; col < grid.ColumnCount(); col++)
                {
                    toPrint += grid.GetValue(row, col);
                }
                toPrint += "\n";
            }

            Console.WriteLine(toPrint);

            int[] goalXY = FindPos(grid, '%');

            int startX = 1;
            int startY = 1;

            List<Cell> path = AStarSearch(grid, startX, startY, goalXY[0], goalXY[1]);
                
            Snake snake = new Snake(new Point(startX, startY), 5);
            snake.Draw();

            int currentX = startX;
            int currentY = startY;
            while (true)
            {
                if (grid.GetValue(snake.GetX(), snake.GetY()) == '%')
                {
                    Random rnd = new Random();
                    int goalX = rnd.Next(1, grid.RowCount() - 2);
                    int goalY = rnd.Next(1, grid.ColumnCount() - 2);
                    while (grid.GetValue(goalX, goalY) == '*')
                    {
                        goalX = rnd.Next(1, grid.RowCount() - 2);
                        goalY = rnd.Next(1, grid.ColumnCount() - 2);
                    }
                    grid.SetValue(goalX, goalY, '%');

                    Console.SetCursorPosition(goalY, goalX);
                    Console.Write('%');

                    path = AStarSearch(grid, currentX, currentY, goalX, goalY);
                }

                foreach (Cell cell in path)
                {
                    int deltaX = cell.X - currentX;
                    int deltaY = cell.Y - currentY;

                    snake.Move(deltaX, deltaY);

                    currentX = cell.X;
                    currentY = cell.Y;

                    Thread.Sleep(100);
                }

                /*ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow && grid.GetValue(snake.GetX() - 1, snake.GetY()) != '*')
                {
                    snake.Move(Snake.Direction.Up);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow && grid.GetValue(snake.GetX() + 1, snake.GetY()) != '*')
                {
                    snake.Move(Snake.Direction.Down);
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow && grid.GetValue(snake.GetX(), snake.GetY() - 1) != '*')
                {
                    snake.Move(Snake.Direction.Left);
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && grid.GetValue(snake.GetX(), snake.GetY() + 1) != '*')
                {
                    snake.Move(Snake.Direction.Right);
                }*/
            }
        }

        public static List<Cell> AStarSearch(Grid<char> grid, int startX, int startY, int goalX, int goalY)
        {
            int gridRows = grid.RowCount();
            int gridColumns = grid.ColumnCount();

            List<Cell> openList = new List<Cell>();
            List<Cell> closedList = new List<Cell>();

            Cell start = new Cell(startX, startY, 0, CalculateHeuristic(startX, startY, goalX, goalY), null);
            Cell goal = new Cell(goalX, goalY, 0, 0, null);

            openList.Add(start);

            while (openList.Count > 0)
            {
                Cell current = openList[0];
                int currentIndex = 0;
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].TotalCost < current.TotalCost)
                    {
                        current = openList[i];
                        currentIndex = i;
                    }
                }

                openList.RemoveAt(currentIndex);
                closedList.Add(current);

                // Om målet är nått, returnera vägen
                if (current.Equals(goal))
                {
                    List<Cell> path = new List<Cell>();
                    while (current != null)
                    {
                        if (path.Contains(current))
                        {
                            Console.WriteLine("Circular reference detected. Unable to find path.");
                            return new List<Cell>();
                        }

                        path.Add(current);
                        current = current.Parent;
                    }

                    //Console.WriteLine($"The Goal is found at: x = {path[0].X}, y = {path[0].Y}");
                    //Thread.Sleep(4000);


                    path.Reverse();

                    return path;
                }

                // (upp, ned, vänster, höger)
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (newX >= 0 && newX < gridRows && newY >= 0 && newY < gridColumns && grid.GetValue(newX, newY) != '*' && !CellInList(newX, newY, closedList))
                    {
                        int newCost = current.GCost + 1;

                        Cell neighbor = new Cell(newX, newY, newCost, CalculateHeuristic(newX, newY, goalX, goalY), current);

                        // Om grann noden redan finns i öppen lista och den har en högre kostnad, ignorera den
                        if (CellInList(newX, newY, openList) && newCost >= neighbor.GCost)
                        {
                            continue;
                        }

                        // INFO: Dessa rader ner till DrwaGrid() är för att visualisera sökningen
                        //Thread.Sleep(60);
                        //Console.Clear();
                        //DrawGrid(grid, newX, newY);

                        // Annars lägg till grannnoden i öppen lista
                        openList.Add(neighbor);
                    }
                }
            }

            // Ingen väg hittades
            return new List<Cell>();
        }

        public static bool CellInList(int x, int y, List<Cell> list)
        {
            foreach (Cell cell in list)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CalculateHeuristic(int x, int y, int goalX, int goalY)
        {
            return Math.Abs(x - goalX) + Math.Abs(y - goalY);
        }

        public static int[] FindPos(Grid<char> grid, char str)
        {
            int gridRows = grid.RowCount();
            int gridColumns = grid.ColumnCount();
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    if (grid.GetValue(row, col) == str)
                    {
                        return new int[] { row, col };
                    }
                }
            }

            return new int[] { 1, 1 };
        }


        public static Grid<char> PopulateEmptyGrid(int gridRows, int gridColumns)
        {
            Grid<char> gridList = new CharGrid(gridRows, gridColumns);

            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    if (row == 0 || row > gridRows - 2 || col == 0 || col > gridColumns - 2)
                    {
                        gridList.SetValue(row, col, '*');
                    }
                    else
                    {
                        gridList.SetValue(row, col, ' ');
                    }
                }
            }

            return gridList;
        }

        
    }

    class Cell
    {
        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }
        public int GCost
        {
            get; set;
        }
        public int HCost
        {
            get; set;
        }
        public int TotalCost
        {
            get
            {
                return GCost + HCost;
            }
        }
        public Cell Parent
        {
            get; set;
        }

        public Cell(int x, int y, int gCost, int hCost, Cell parent)
        {
            X = x;
            Y = y;
            GCost = gCost;
            HCost = hCost;
            Parent = parent;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Cell other = (Cell)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
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

        public void Move(int dx, int dy)
        {
            if (dx != 0)
            {
                CurrentDirection = dx > 0 ? Direction.Up : Direction.Down;
            }
            else if (dy != 0)
            {
                CurrentDirection = dy > 0 ? Direction.Left : Direction.Right;
            }

            SnakePart newHead = new SnakePart(new Point(Head.Position.X + dx, Head.Position.Y + dy), Symbol);

            Tail.Erase();
            for (var i = _body.Length - 1; i > 0; i--)
            {
                _body[i] = _body[i - 1];
            }
            _body[0] = newHead;
            Draw();
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

        public int GetX()
        {
            return Head.Position.X;
        }

        public int GetY()
        {
            return Head.Position.Y;
        }
    }

    public abstract class Grid<T>
    {
        private T[,] gridRepository;

        public Grid(int rows, int columns)
        {
            gridRepository = new T[rows, columns];
        }

        virtual public T GetValue(int rowNumber, int columnNumber)
        {
            return gridRepository[rowNumber, columnNumber];
        }

        virtual public void SetValue(int rowNumber, int columnNumber, T inputItem)
        {
            gridRepository[rowNumber, columnNumber] = inputItem;
        }

        virtual public int RowCount()
        {
            return gridRepository.GetLength(0);
        }

        virtual public int ColumnCount()
        {
            return gridRepository.GetLength(1);
        }
    }

    public class CharGrid : Grid<char>
    {
        public CharGrid(int rows, int columns) : base(rows, columns)
        {
        }
    }
}


