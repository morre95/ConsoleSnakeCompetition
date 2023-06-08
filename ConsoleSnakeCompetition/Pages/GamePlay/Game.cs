using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Algorithms;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Menu;
using ConsoleSnakeCompetition.Classes.Player;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Pages.Admin;
using ConsoleSnakeCompetition.Utilities;


namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class Game
    {

        public static void Init()
        {
            Console.Clear();
            List<string> ascii = new List<string>() {
            @" /$$$$$$                      /$$                 ",
            @" /$$__  $$                    | $$                ",
            @"| $$  \__/ /$$$$$$$   /$$$$$$ | $$   /$$  /$$$$$$ ",
            @"|  $$$$$$ | $$__  $$ |____  $$| $$  /$$/ /$$__  $$",
            @" \____  $$| $$  \ $$  /$$$$$$$| $$$$$$/ | $$$$$$$$",
            @" /$$  \ $$| $$  | $$ /$$__  $$| $$_  $$ | $$_____/",
            @"|  $$$$$$/| $$  | $$|  $$$$$$$| $$ \  $$|  $$$$$$$",
            @" \______/ |__/  |__/ \_______/|__/  \__/ \_______/"};

            ascii.ForEach(Console.WriteLine);

            Menu menu = new Menu(
                new Option("Start"),
                new Option("Settings", Config.Init),
                new Option("Scoreboard", NotImplementedException)
                );

            menu.Display();

            Console.Clear();
            Grid<char> grid = Load.SelectGrid();

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


            Run(grid, AppSettings.Instance.GetDelayMS());
        }

        private static void NotImplementedException()
        {
            throw new NotImplementedException();
            ScoreBoard score = new TopScoreBoard(
                new PlayerScore("Kalle", 113, DateTime.Now),
                new PlayerScore("Foo", 99, DateTime.Parse("2023-02-05 12:22:11"))
                
                );

            Console.Clear();
            Console.WriteLine(score);
            WaitTermination();
            Init();
        }


        static void Run(Grid<char> grid, int delayMS)
        {
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

            Stack<Cell> path = AStar.Search(grid, compStartX, compStartY, goalX, goalY);

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
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            if (grid.GetValue(snake.GetX() + 1, snake.GetY()) != '*')
                                snake.Move(Snake.Direction.Down);
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() - 1) != '*')
                                snake.Move(Snake.Direction.Left);
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() + 1) != '*')
                                snake.Move(Snake.Direction.Right);
                            break;
                    }

                    if (grid.GetValue(snake.GetX(), snake.GetY()) == '%')
                    {
                        grid.SetValue(snake.GetX(), snake.GetY(), ' ');

                        GenerateRandomXY(grid, out goalX, out goalY);
                        SetNewGoal(grid, goalX, goalY);

                        path = AStar.Search(grid, currentX, currentY, goalX, goalY);

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

                        path = AStar.Search(grid, currentX, currentY, goalX, goalY);
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
            Init();
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

        public static void WaitTermination()
        {
            Console.Write("Press any key to terminate ...");
            Console.ReadKey();
        }
    }
}

