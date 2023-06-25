using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class Game2P
    {

        public static void InitRun()
        {
            Run();
        }

        // TBD: Få ett extra poäng om man äter upp andra ormens kropp
        public static string Run()
        {
            string player1Name = AppSettings.Instance.Player1Name;
            string player2Name = AppSettings.Instance.Player2Name;

            Console.Clear();
            int rows = Console.WindowHeight - 2;
            int cols = Console.WindowWidth - 2;

            Grid<char> grid = Load.PopulateEmptyGrid(rows, cols);

            DrawGrid(grid, ConsoleColor.Magenta);


            var snake1 = new Snake(new Point(1, 1), 5);
            snake1.Draw();

            var snake2 = new Snake(new Point(rows - 2, cols - 2), 5, drawColorized: true);
            snake2.Draw();

            int goalRow, goalCol, goalPoint;
            SpawnRandomFood(grid, rows, cols, out goalRow, out goalCol, out goalPoint);

            Console.CursorVisible = false;
            ConsoleKey key;
            int i = 0;

            int snake1Score = 0;
            int snake2Score = 0;

            while (true)
            {
                if (snake1.Length == 1)
                {
                    snake1.Loose();
                    break;
                }

                if (snake2.Length == 1)
                {
                    snake2.Loose();
                    break;
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                {
                    break;
                }


                if (i % 2 == 0)
                {
                    if (key == ConsoleKey.W)
                    {
                        if (grid.GetValue(snake1.GetX() - 1, snake1.GetY()) != '*')
                        {
                            if (snake1.IsEatingPart(Snake.Direction.Up))
                            {
                                snake1.ReduceLength(1);
                            }
                            else
                            {
                                snake1.Move(Snake.Direction.Up);
                            }

                            i++;
                        }
                            
                    }
                    if (key == ConsoleKey.S)
                    {
                        if (grid.GetValue(snake1.GetX() + 1, snake1.GetY()) != '*')
                        {
                            if (snake1.IsEatingPart(Snake.Direction.Down))
                            {
                                snake1.ReduceLength(1);
                            }
                            else
                            {
                                snake1.Move(Snake.Direction.Down);
                            }

                            i++;
                        }
                    }
                    if (key == ConsoleKey.A)
                    {
                        if (grid.GetValue(snake1.GetX(), snake1.GetY() - 1) != '*')
                        {
                            if (snake1.IsEatingPart(Snake.Direction.Left))
                            {
                                snake1.ReduceLength(1);
                            }
                            else
                            {
                                snake1.Move(Snake.Direction.Left);
                            }

                            i++;
                        }
                    }
                    if (key == ConsoleKey.D)
                    {
                        if (grid.GetValue(snake1.GetX(), snake1.GetY() + 1) != '*')
                        {
                            if (snake1.IsEatingPart(Snake.Direction.Right))
                            {
                                snake1.ReduceLength(1);
                            }
                            else
                            {
                                snake1.Move(Snake.Direction.Right);
                            }

                            i++;
                        }
                    }
                    if (snake1.GetX() == snake2.GetX() && snake1.GetY() == snake2.GetY())
                    {
                        snake2.ReduceLength(snake2.Length - 1);
                        snake2.Loose();
                        snake2Score = 0;
                        break;
                    }
                }
                else
                {
                    if (key == ConsoleKey.UpArrow)
                    {
                        if (grid.GetValue(snake2.GetX() - 1, snake2.GetY()) != '*')
                        {
                            if (snake2.IsEatingPart(Snake.Direction.Up))
                            {
                                snake2.ReduceLength(1);
                            }
                            else
                            {
                                snake2.Move(Snake.Direction.Up);
                            }

                            i++;
                        }
                    }
                    if (key == ConsoleKey.DownArrow)
                    {
                        if (grid.GetValue(snake2.GetX() + 1, snake2.GetY() ) != '*')
                        {
                            if (snake2.IsEatingPart(Snake.Direction.Down))
                            {
                                snake2.ReduceLength(1);
                            }
                            else
                            {
                                snake2.Move(Snake.Direction.Down);
                            }

                            i++;
                        }
                    }
                    if (key == ConsoleKey.LeftArrow)
                    {
                        if (grid.GetValue(snake2.GetX(), snake2.GetY() - 1) != '*')
                        {
                            if (snake2.IsEatingPart(Snake.Direction.Left))
                            {
                                snake2.ReduceLength(1);
                            }
                            else
                            {
                                snake2.Move(Snake.Direction.Left);
                            }

                            i++;
                        }
                    }
                    if (key == ConsoleKey.RightArrow)
                    {
                        if (grid.GetValue(snake2.GetX(), snake2.GetY() + 1) != '*')
                        {
                            if (snake2.IsEatingPart(Snake.Direction.Right))
                            {
                                snake2.ReduceLength(1);
                            }
                            else
                            {
                                snake2.Move(Snake.Direction.Right);
                            }

                            i++;
                        }
                    }

                    if (snake1.GetX() == snake2.GetX() && snake1.GetY() == snake2.GetY())
                    {
                        // TBD: Fixa något annat sätt att nolla poängen än att sätta´dra av längden och sätta score till 0. Det blir lika med 1 i resultatet, borde vara 0
                        // TBD: Tex. Skapa en points attribut i snake klassen...
                        snake1.ReduceLength(snake1.Length - 1);
                        snake1.Loose();
                        snake1Score = 0;
                        break;
                    }
                }

                
                if (snake1.GetX() == goalRow && snake1.GetY() == goalCol)
                {
                    snake1.AddLength(goalPoint);
                    snake1Score += goalPoint;
                    SpawnRandomFood(grid, rows, cols, out goalRow, out goalCol, out goalPoint);
                }

                if (snake2.GetX() == goalRow && snake2.GetY() == goalCol)
                {
                    snake2.AddLength(goalPoint);
                    snake2Score += goalPoint;
                    SpawnRandomFood(grid, rows, cols, out goalRow, out goalCol, out goalPoint);
                }

                Output.WriteOnBottomLine($"{player1Name} length: {snake1.Length,2} and score: {snake1Score}, {player2Name} length: {snake2.Length,2} and score: {snake2Score}");
            }
            Console.Clear();
            Output.WriteLine(ConsoleColor.Magenta, $"The game result is:");
            Console.WriteLine($"{player1Name} total score: {snake1.Length + snake1Score}, {player2Name} total score: {snake2.Length + snake2Score}");
            if (snake1.Length + snake1Score > snake2.Length + snake2Score)
            {
                Console.WriteLine($"{player1Name} won");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return player1Name;
            }
            else if (snake1.Length + snake1Score == snake2.Length + snake2Score)
            {
                Console.WriteLine("It was a draw");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return null;
            }
            else
            {
                Console.WriteLine($"{player2Name} won");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return player2Name;
            }
            
        }

        private static void SpawnRandomFood(Grid<char> grid, int rows, int cols, out int goalRow, out int goalCol, out int goalPoint)
        {
            Random rnd = new Random();
            goalRow = rnd.Next(1, rows - 2);
            goalCol = rnd.Next(1, cols - 2);
            goalPoint = GetRandomFood();
            grid.SetValue(goalRow, goalCol, Convert.ToChar(goalPoint));
            Output.WriteAt(ConsoleColor.Yellow, goalPoint, goalCol, goalRow);
        }

        static readonly List<KeyValuePair<double, int>> ProbabilityFoods = new List<KeyValuePair<double, int>>
        {
            new KeyValuePair<double, int>(0.1, 10),
            new KeyValuePair<double, int>(3, 4),
            new KeyValuePair<double, int>(10, 3),
            new KeyValuePair<double, int>(30, 2),
            new KeyValuePair<double, int>(50, 1)
        };

        private static int GetRandomFood()
        {
            Random rand = new Random();
            double randomValue = rand.NextDouble() * 100;

            foreach (KeyValuePair<double, int> pair in ProbabilityFoods)
            {
                if (randomValue < pair.Key)
                {
                    return Convert.ToChar(pair.Value);
                }
            }
            return 1;
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
    }
}

