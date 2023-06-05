using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using ConsoleSnakeCompetition.Classes;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int rows = 20, cols = 80;
            //Grid<char> grid = PopulateEmptyGrid(rows, cols);
            Grid<char> grid = LoadFromFile(@"grids\bigTest.json");

            Random rnd = new Random();
            int goalX = rnd.Next(1, grid.RowCount() - 2);
            int goalY = rnd.Next(1, grid.ColumnCount() - 2);
            while (grid.GetValue(goalX, goalY) == '*')
            {
                goalX = rnd.Next(1, grid.RowCount() - 2);
                goalY = rnd.Next(1, grid.ColumnCount() - 2);
            }
            grid.SetValue(goalX, goalY, '%');

            DrawGrid(grid);

            int[] goalXY = FindPos(grid, '%');

            int startX = 1;
            int startY = 1;

            Stack<Cell> path = AStarSearch(grid, startX, startY, goalXY[0], goalXY[1]);
                
            Snake snake = new Snake(new Point(startX, startY), 5);
            snake.Draw();

            Snake computer = new Snake(new Point(startX, startY), 5, '^');
            computer.Draw();

            int currentX = startX;
            int currentY = startY;
            Console.CursorVisible = false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                if (grid.GetValue(snake.GetX(), snake.GetY()) == '%')
                {
                    grid.SetValue(snake.GetX(), snake.GetY(), ' ');

                    goalX = rnd.Next(1, grid.RowCount() - 2);
                    goalY = rnd.Next(1, grid.ColumnCount() - 2);
                    while (grid.GetValue(goalX, goalY) == '*')
                    {
                        goalX = rnd.Next(1, grid.RowCount() - 2);
                        goalY = rnd.Next(1, grid.ColumnCount() - 2);
                    }
                    grid.SetValue(goalX, goalY, '%');

                    Console.SetCursorPosition(goalY, goalX);
                    Console.Write('%');

                    path = AStarSearch(grid, currentX, currentY, goalX, goalY);

                    snake.AddLength(4);
                }

                if (stopwatch.ElapsedMilliseconds >= 100 && path.Any())
                {
                    Cell cell = path.Pop();

                    int deltaX = cell.X - currentX;
                    int deltaY = cell.Y - currentY;

                    computer.Move(deltaX, deltaY);

                    currentX = cell.X;
                    currentY = cell.Y;

                    stopwatch.Restart();

                    if (grid.GetValue(computer.GetX(), computer.GetY()) == '%')
                    {
                        grid.SetValue(computer.GetX(), computer.GetY(), ' ');

                        goalX = rnd.Next(1, grid.RowCount() - 2);
                        goalY = rnd.Next(1, grid.ColumnCount() - 2);
                        while (grid.GetValue(goalX, goalY) == '*')
                        {
                            goalX = rnd.Next(1, grid.RowCount() - 2);
                            goalY = rnd.Next(1, grid.ColumnCount() - 2);
                        }
                        grid.SetValue(goalX, goalY, '%');

                        Console.SetCursorPosition(goalY, goalX);
                        Console.Write('%');

                        path = AStarSearch(grid, currentX, currentY, goalX, goalY);

                        computer.AddLength(1);
                    }
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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
                    }
                } 
            }
        }

        private static void DrawGrid(Grid<char> grid)
        {
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
        }

        public static Stack<Cell> AStarSearch(Grid<char> grid, int startX, int startY, int goalX, int goalY)
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
                    Stack<Cell> path = new Stack<Cell>();
                    while (current != null)
                    {
                        if (path.Contains(current))
                        {
                            Console.WriteLine("Circular reference detected. Unable to find path.");
                            return new Stack<Cell>();
                        }

                        path.Push(current);
                        current = current.Parent;
                    }

                    //Console.WriteLine($"The Goal is found at: x = {path[0].X}, y = {path[0].Y}");
                    //Thread.Sleep(4000);

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
            return new Stack<Cell>();
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

        public static Grid<char> LoadFromFile(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);

            List<List<string>> gridList = JsonSerializer.Deserialize<List<List<string>>>(jsonString)!;

            Grid<char> grid = new CharGrid(gridList.Count, gridList[0].Count);
            for (int row = 0; row < gridList.Count; row++)
            {
                for (int col = 0; col < gridList[row].Count; col++)
                {
                    grid.SetValue(row, col, char.Parse(gridList[row][col]));
                }
            }

            return grid;
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
}


