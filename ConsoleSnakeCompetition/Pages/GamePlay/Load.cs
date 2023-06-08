using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class Load
    {

        private static readonly string gridsPath = Path.GetFullPath(@"Resources\Grids\");

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

        public static void SaveToFile(Grid<char> grid)
        {
            Console.Write("Filename: ");
            string fileName = Console.ReadLine()!;

            if (!Directory.Exists(gridsPath)) Directory.CreateDirectory(gridsPath);

            List<List<char>> gridList = new List<List<char>>();
            for (int row = 0; row < grid.RowCount(); row++)
            {
                List<char> newRow = new List<char>();
                for (int col = 0; col < grid.ColumnCount(); col++)
                {
                    newRow.Add(grid.GetValue(row, col));
                }
                gridList.Add(newRow);
            }


            string jsonString = JsonSerializer.Serialize(gridList);
            File.WriteAllText(gridsPath + fileName + ".json", jsonString);
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

        public static Grid<char> SelectGrid()
        {
            string[] files = Directory.GetFiles(gridsPath, "*.json");
            Grid<char> grid = new CharGrid(0, 0);

            if (files.Length == 1)
            {
                grid = LoadFromFile(Path.GetFileName(files[0]));
            }
            else if (files.Length > 1)
            {
                var choice = 0;
                ConsoleKey key;
                do
                {
                    for (var i = 0; i < files.Length; i++)
                    {
                        Console.SetCursorPosition(0, i);

                        if (i == choice)
                        {
                            Output.Write(ConsoleColor.Red, Path.GetFileName(files[i]));
                        }
                        else
                        {
                            Console.Write(Path.GetFileName(files[i]));
                        }
                    }

                    key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            if (choice > 0) choice--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (choice < files.Length - 1) choice++;
                            break;
                    }

                } while (key != ConsoleKey.Enter);

                grid = LoadFromFile(Path.GetFileName(files[choice]));
            }
            else
            {
                var cols = Console.WindowWidth;
                var rows = Console.WindowHeight;
                grid = PopulateEmptyGrid(rows - 2, cols - 2);
            }

            return grid;
        }

        public static string[] GetJsonFiles()
        {
            return Directory.GetFiles(gridsPath, "*.json", SearchOption.AllDirectories);
        }
    }
}

