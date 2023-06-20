﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class Game2P
    {
        public static void Run()
        {
            Console.Clear();
            int rows = Console.WindowHeight - 2;
            int cols = Console.WindowWidth - 2;

            /*for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (row == 0 || row > rows - 2 || col == 0 || col > cols - 2)
                    {
                        Output.WriteAt(ConsoleColor.Green, "*", col, row);
                    }
                }
                Console.WriteLine();
            }*/

            // FIXME: Fortstätt att porta över från enbart top, left cords till Grid<char> x, y
            Grid<char> grid = Load.PopulateEmptyGrid(rows, cols);

            DrawGrid(grid, ConsoleColor.Green);


            var snake1 = new Snake(new Point(11, 11), 30);
            snake1.Draw();

            var snake2 = new Snake(new Point(15, 15), 30, drawColorized: true);
            snake2.Draw();

            int goalRow, goalCol, goalPoint;
            SpawnRandomFood(rows, cols, out goalRow, out goalCol, out goalPoint);

            Console.CursorVisible = false;
            ConsoleKey key;
            int i = 0;

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
                if (i % 2 == 0)
                {
                    if (key == ConsoleKey.W)
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
                    if (key == ConsoleKey.S)
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
                    if (key == ConsoleKey.A)
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
                    if (key == ConsoleKey.D)
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
                else
                {
                    if (key == ConsoleKey.UpArrow)
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
                    if (key == ConsoleKey.DownArrow)
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
                    if (key == ConsoleKey.LeftArrow)
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
                    if (key == ConsoleKey.RightArrow)
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

                int snake1Score = 0;
                int snake2Score = 0;
                if (snake1.GetX() == goalRow && snake1.GetY() == goalCol)
                {
                    snake1.AddLength(goalPoint);
                    snake1Score += goalPoint;
                    SpawnRandomFood(rows, cols, out goalRow, out goalCol, out goalPoint);
                }

                if (snake2.GetX() == goalRow && snake2.GetY() == goalCol)
                {
                    snake2.AddLength(goalPoint);
                    snake2Score += goalPoint;
                    SpawnRandomFood(rows, cols, out goalRow, out goalCol, out goalPoint);
                }

                Output.WriteOnBottomLine($"Snake 1 length: {snake1.Length,2} and score: {snake1Score}, Snake 2 length: {snake2.Length,2} and score: {snake2Score}");
            }
        }

        private static void SpawnRandomFood(int rows, int cols, out int goalRow, out int goalCol, out int goalPoint)
        {
            Random rnd = new Random();
            goalRow = rnd.Next(1, rows - 2);
            goalCol = rnd.Next(1, cols - 2);
            goalPoint = GetRandomFood();
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
