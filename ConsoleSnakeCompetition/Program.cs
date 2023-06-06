﻿using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Menu;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {

        static void Main(string[] args)
        {

            /*Logger<AppSettings>.Instance.Warn("Värdet måste varnas");
            Logger<AppSettings>.Instance.Error("Error");
            Logger<Snake>.Instance.Trace("Trace the snake");
            Logger<Menu>.Instance.Debug("Nu debuggar vi menu");
            Logger<AppSettings>.Instance.Success("Yes det funkade");
            return;*/

            AppSettings.Instance.LoadSettings();

            InitGame();
        }

        static void InitGame()
        {
            Menu menu = new Menu(
                new Option("Start"),
                new Option("Speed", SetSpeed)
                );

            menu.Display();

            for (int i = 3; i > 0; i--)
            {
                Console.Clear();
                Console.WriteLine($"Selected speed: {AppSettings.Instance.Speed}");
                Console.WriteLine($"Ready? {new string('.', i)} ");
                Thread.Sleep(850);
            }
            Console.Clear();
            Console.WriteLine("GO!!!");
            Thread.Sleep(600);
            Console.Clear();

            Run(AppSettings.Instance.GetDelayMS());
        }

        private static void SetSpeed()
        {
            int stepCount = AppSettings.Instance.StepSpeedCount;
            int selectedValue = AppSettings.Instance.Speed;

            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                Console.Write($"Speed: {selectedValue}");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                {
                    selectedValue++;
                    if (selectedValue > stepCount) { selectedValue = 1; }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedValue--;
                    if (selectedValue < 1) { selectedValue = stepCount; }
                }
                else if (key == ConsoleKey.Enter)
                {
                    break;
                }
            }

            AppSettings.Instance.Speed = selectedValue;

            AppSettings.Instance.SaveSettings();
            InitGame();
        }

        private static readonly string gridsPath = Path.GetFullPath(@"Resources\Grids\");

        static void Run(int delayMS)
        {
            //int rows = 20, cols = 80;
            //Grid<char> grid = PopulateEmptyGrid(rows, cols);
            string fileName = "bigTest.json";
            Grid<char> grid = LoadFromFile(fileName);

            DrawGrid(grid);

            int goalX, goalY;
            GenerateRandomXY(grid, out goalX, out goalY);
            SetNewGoal(grid, goalX, goalY);

            int startX, startY;
            GenerateRandomXY(grid, out startX, out startY);

            Snake snake = new Snake(new Point(startX, startY), 5, '#', true);
            snake.Draw();

            int compStartX, compStartY;
            GenerateRandomXY(grid, out compStartX, out compStartY);

            Snake computer = new Snake(new Point(compStartX, compStartY), 5, '?');
            computer.Draw();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stack<Cell> path = AStarSearch(grid, compStartX, compStartY, goalX, goalY);

            int currentX = compStartX;
            int currentY = compStartY;

            int score = 0;
            int computerScore = 0;

            Console.CursorVisible = false;

            while (true)
            {

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q) break;

                    switch (key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            if (grid.GetValue(snake.GetX() - 1, snake.GetY()) != '*')
                                snake.Move(Snake.Direction.Up);
                            //snake.Move(-1, 0);
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            if (grid.GetValue(snake.GetX() + 1, snake.GetY()) != '*')
                                snake.Move(Snake.Direction.Down);
                            //snake.Move(1, 0);
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() - 1) != '*')
                                snake.Move(Snake.Direction.Left);
                            //snake.Move(0, -1);
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() + 1) != '*')
                                snake.Move(Snake.Direction.Right);
                            //snake.Move(0, 1);
                            break;
                    }

                    if (grid.GetValue(snake.GetX(), snake.GetY()) == '%')
                    {
                        grid.SetValue(snake.GetX(), snake.GetY(), ' ');

                        GenerateRandomXY(grid, out goalX, out goalY);
                        SetNewGoal(grid, goalX, goalY);

                        path = AStarSearch(grid, currentX, currentY, goalX, goalY);

                        snake.AddLength(1);

                        Console.SetCursorPosition(0, grid.RowCount() + 1);
                        score++;
                        Console.Write($"Score: (you/computer) {score}/{computerScore}");
                    }
                }

                if (stopwatch.ElapsedMilliseconds >= delayMS && path.Any())
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
                        GenerateRandomXY(grid, out goalX, out goalY);
                        SetNewGoal(grid, goalX, goalY);

                        path = AStarSearch(grid, currentX, currentY, goalX, goalY);
                        computer.AddLength(1);

                        Console.SetCursorPosition(0, grid.RowCount() + 1);
                        computerScore++;
                        Console.Write($"Score: (you/computer) {score}/{computerScore}");
                    }
                }
            }

            Console.Clear();
            if (score > computerScore)
            {
                Console.WriteLine("Congrats you won");
                Console.WriteLine($"Score: (you/computer) {score}/{computerScore}");
            }
            else if (score == computerScore)
            {
                Console.WriteLine("It was a draw");
                Console.WriteLine($"Score: (you/computer) {score}/{computerScore}");
            }
            else
            {
                Console.WriteLine("You loose");
                Console.WriteLine($"Score: (you/computer) {score}/{computerScore}");
            }

            WaitTermination();
        }

        static void WaitTermination()
        {
            Console.Write("Press any key to terminate ...");
            Console.ReadKey();
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

        private static void SetNewGoal(Grid<char> grid, int goalX, int goalY)
        {
            grid.SetValue(goalX, goalY, '%');
            Console.SetCursorPosition(goalY, goalX);
            Console.Write('%');
        }

        private static void GenerateRandomXY(Grid<char> grid, out int x, out int y)
        {
            Random rnd = new Random();
            x = rnd.Next(1, grid.RowCount() - 2);
            y = rnd.Next(1, grid.ColumnCount() - 2);
            while (grid.GetValue(x, y) == '*')
            {
                x = rnd.Next(1, grid.RowCount() - 2);
                y = rnd.Next(1, grid.ColumnCount() - 2);
            }
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
            string jsonString = File.ReadAllText(gridsPath + fileName);

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


