using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
                new Option("Start", InitRun),
                new Option("Two Player", Game2P.InitRun),
                new Option("Tournament", GameTournament.Run),
                new Option("Settings", Config.Init),
                new Option("Score Board", ScoreBoard)
                );

            Output.WriteOnBottomLine(ConsoleColor.Green, $"v {GameVersion}");

            menu.Display();

            
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

        static void InitRun()
        {
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

            var totalTime = SetInterval(() => delayMS += delayMS >= AppSettings.Instance.MinSpeedValue ? -20 : 0, 1000 * AppSettings.Instance.IncreaseSpeedEvery);

            while (score + computerScore < bestOfRounds && (score < bestOfRounds) && (computerScore < bestOfRounds))
            {
                if (snake.Length <= 1) 
                {
                    snake.Loose();
                    break; 
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q || key == ConsoleKey.Escape) break;

                    char aheadChar;
                    switch (key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            aheadChar = grid.GetValue(snake.GetXAhead(Snake.Direction.Up), snake.GetYAhead(Snake.Direction.Up));
                            if (aheadChar != '*')
                            {
                                score = CheckIfScore(grid, Snake.Direction.Up, snake, score, aheadChar);

                                if (snake.IsEatingPart(Snake.Direction.Up))
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }

                                snake.Move(Snake.Direction.Up);
                            }

                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            aheadChar = grid.GetValue(snake.GetXAhead(Snake.Direction.Down), snake.GetYAhead(Snake.Direction.Down));
                            if (aheadChar != '*')
                            {
                                score = CheckIfScore(grid, Snake.Direction.Down, snake, score, aheadChar);

                                if (snake.IsEatingPart(Snake.Direction.Down))
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                                snake.Move(Snake.Direction.Down);
                            }
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            aheadChar = grid.GetValue(snake.GetXAhead(Snake.Direction.Left), snake.GetYAhead(Snake.Direction.Left));
                            if (aheadChar != '*')
                            {
                                score = CheckIfScore(grid, Snake.Direction.Left, snake, score, aheadChar);

                                if (snake.IsEatingPart(Snake.Direction.Left))
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                                snake.Move(Snake.Direction.Left);
                            }
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            aheadChar = grid.GetValue(snake.GetXAhead(Snake.Direction.Right), snake.GetYAhead(Snake.Direction.Right));
                            if (aheadChar != '*')
                            {
                                score = CheckIfScore(grid, Snake.Direction.Right, snake, score, aheadChar);

                                if (snake.IsEatingPart(Snake.Direction.Right))
                                {
                                    snake.ReduceLength(1);
                                    score--;
                                }
                                snake.Move(Snake.Direction.Right);
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

                    // TBD: Kanske leta igenom kartan vid uppstart och sök efter siffror eller efter en viss tid
                    if (int.TryParse(grid.GetValue(computer.GetX(), computer.GetY()).ToString(), out int points))
                    {
                        computerScore *= points;
                    }

                }

                Console.SetCursorPosition(0, grid.RowCount() + 1);
                Console.Write($"Score: (you/computer) {score}/{computerScore}, delayMS = {delayMS}");

            }

            totalTime.Dispose();


            Console.Clear();

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

        private static int CheckIfScore(Grid<char> grid, Snake.Direction direction, Snake snake, int score, char aheadChar)
        {
            if (int.TryParse(aheadChar.ToString(), out int points))
            {
                grid.SetValue(snake.GetXAhead(direction), snake.GetYAhead(direction), ' ');
                var rand = new Random();
                switch (rand.Next(11)) // = 1-10
                {
                    case 1:
                    case 2:
                        score *= points;
                        break;
                    case 3:
                    case 4:
                        score -= points;
                        break;
                    default: 
                        score += points;
                        break;
                }
                snake.ReduceLength(points);
            }

            return score;
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

