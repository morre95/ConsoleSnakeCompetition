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
        private const int _major = 0;
        private const int _minor = 0;
        private const int _patch = 1500;

        public static string GameVersion => string.Format("{0:00}.{1:00}.{2:0000}", _major, _minor, _patch);

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
                new Option("Score Board", ScoreBoard)
                );

            Output.WriteOnBottomLine(ConsoleColor.Green, $"v {GameVersion}");

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


            Run(grid);
        }

        private static void ScoreBoard()
        {
            ScoreBoard score = new TopScoreBoard();
            score.Load();

            Console.Clear();
            Console.WriteLine(score);
            WaitTermination();
            Init();
        }


        static void Run(Grid<char> grid)
        {
            ConsoleColor consoleColor;

            if (!Enum.TryParse(AppSettings.Instance.ThemeColor, out consoleColor))
            {
                Console.WriteLine("Invalid Theme color, change that in settings");
                Console.WriteLine("Press any key to continue to settings...");
                Console.ReadLine();
                Config.SelectThemeColor();
                return;
            }

            DrawGrid(grid, consoleColor);

            int goalX, goalY;
            GenerateRandomXY(grid, out goalX, out goalY);
            SetNewGoal(grid, goalX, goalY);

            int startX, startY;
            GenerateRandomXY(grid, out startX, out startY);

            Snake snake = new Snake(new Point(startX, startY), 
                AppSettings.Instance.Player1StartLength, 
                AppSettings.Instance.Player1Symbol, 
                AppSettings.Instance.Player1Colorized, 
                AppSettings.Instance.Player1ColorInverted);
            snake.Draw();

            int compStartX, compStartY;
            GenerateRandomXY(grid, out compStartX, out compStartY);

            Snake computer = new Snake(new Point(compStartX, compStartY), 
                AppSettings.Instance.ComputerStartLength,
                AppSettings.Instance.ComputerSymbol,
                AppSettings.Instance.ComputerColorized,
                AppSettings.Instance.ComputerColorInverted);
            computer.Draw();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stack<Cell> path = AStar.Search(grid, compStartX, compStartY, goalX, goalY);

            int currentX = compStartX;
            int currentY = compStartY;

            int score = 0;
            int computerScore = 0;

            Console.CursorVisible = false;

            int delayMS = AppSettings.Instance.GetDelayMS();

            int bestOfRounds = AppSettings.Instance.BestOf == 0 ? int.MaxValue : AppSettings.Instance.BestOf;

            var totalTime = SetInterval(() => delayMS -= 20, 1000 * 60); // Every minute speed up

            while (score + computerScore < bestOfRounds && (score < bestOfRounds) && (computerScore < bestOfRounds))
            {
                if (snake.Length <= 1) 
                { 
                    /*int x = snake.GetX();
                    int y = snake.GetY();
                    for (int i = 0; i < 5; i++)
                    {
                        Console.SetCursorPosition(y, x);
                        Output.Write(ConsoleColor.Red, '#');
                        Console.SetCursorPosition(y, x);

                        Thread.Sleep(500);
                        Console.Write(" ");
                        Thread.Sleep(500);
                    }*/

                    snake.Loose();

                    break; 
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q) break;

                    switch (key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            if (grid.GetValue(snake.GetX() - 1, snake.GetY()) != '*')
                            {
                                if (!snake.IsEatingPart(Snake.Direction.Up))
                                {
                                    snake.Move(Snake.Direction.Up);
                                }
                                else
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                            }
                                
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            if (grid.GetValue(snake.GetX() + 1, snake.GetY()) != '*')
                            {
                                if (!snake.IsEatingPart(Snake.Direction.Down))
                                {
                                    snake.Move(Snake.Direction.Down);
                                }
                                else
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                            }
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() - 1) != '*')
                            {
                                if (!snake.IsEatingPart(Snake.Direction.Left))
                                {
                                    snake.Move(Snake.Direction.Left);
                                }
                                else
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                            }
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (grid.GetValue(snake.GetX(), snake.GetY() + 1) != '*')
                            {
                                if (!snake.IsEatingPart(Snake.Direction.Right))
                                {
                                    snake.Move(Snake.Direction.Right);
                                }
                                else
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                            }
                            break;
                    }

                    if (grid.GetValue(snake.GetX(), snake.GetY()) == '%')
                    {
                        grid.SetValue(snake.GetX(), snake.GetY(), ' ');

                        GenerateRandomXY(grid, out goalX, out goalY);
                        SetNewGoal(grid, goalX, goalY);

                        path = AStar.Search(grid, currentX, currentY, goalX, goalY);

                        snake.AddLength(1);
                        score++;
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
                        computerScore++; 
                    }
                }

                Console.SetCursorPosition(0, grid.RowCount() + 1);
                Console.Write($"Score: (you/computer) {score}/{computerScore}, delayMS = {delayMS}");

            }

            totalTime.Dispose();


            Console.Clear();
            // TBD: Infinity symbolen ser ut som en 8 när man skriver ut den
            //string infinitySymbol = "\u221E";
            //string bestOfStr = AppSettings.Instance.BestOf == 0 ? $"{infinitySymbol}, total rounds: {score + computerScore}" : AppSettings.Instance.BestOf.ToString();

            string bestOfStr = bestOfRounds.ToString();
            Console.WriteLine($"Best of: {bestOfStr}");

            if(snake.Length <= 1) { Console.WriteLine("You eat your self to death"); }

            if (score > computerScore)
            {
                Console.WriteLine("Congrats you won");
                Console.WriteLine($"Score: (you/computer) {score}/{computerScore}");

                ScoreBoard scoreBoard = new TopScoreBoard();
                scoreBoard.Load();

                var newScore = new PlayerScore("Player1", score, DateTime.Now);
                if (scoreBoard.IsHighScoreWorthy(newScore))
                {
                    newScore.PlayerName = Output.ReadLine(ConsoleColor.Yellow, "New High Score: ");

                    scoreBoard.Add(newScore);
                    scoreBoard.Save();
                    // TBD: Highlighta den nyligen tillagda topp poängen
                    ScoreBoard();
                    return;
                }

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

        public static System.Timers.Timer SetInterval(Action Act, int Interval)
        {
            System.Timers.Timer tmr = new System.Timers.Timer();
            tmr.Elapsed += (sender, args) => Act();
            tmr.AutoReset = true;
            tmr.Interval = Interval;
            tmr.Start();

            return tmr;
        }

        private static void DrawGrid(Grid<char> grid, ConsoleColor consoleColor)
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
            
            //Console.WriteLine(toPrint);
            Output.WriteLine(consoleColor, toPrint);
        }

        private static void SetNewGoal(Grid<char> grid, int goalX, int goalY)
        {
            grid.SetValue(goalX, goalY, '%');
            Console.SetCursorPosition(goalY, goalX);
            Output.Write(AppSettings.Instance.GetFoodColor(), '%');
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
            Console.Write("Press Enter to terminate ...");
            Console.ReadLine();
        }
    }
}

