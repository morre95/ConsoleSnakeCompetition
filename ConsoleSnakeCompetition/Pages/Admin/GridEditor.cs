using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Pages.GamePlay;

namespace ConsoleSnakeCompetition.Pages.Admin
{
    internal static class GridEditor
    {
        public static void Init()
        {
            (int rows, int columns) = SelectRowsAndColumns();
            Grid<char> gridList = PopulateEmptyGrid(rows, columns);
            RunEditor(gridList);
        }

        private static (int, int) SelectRowsAndColumns(int rows = 0, int columns = 0)
        {
            return SelectNumber("Rows:", "Columns:", 101, 101, rows, columns);
        }

        private static (int, int) SelectNumber(string text1, string text2, int maxNum1 = 100, int maxNum2 = 100, int startWith1 = 0, int startWith2 = 0)
        {
            int index1 = startWith1;
            int index2 = startWith2;
            bool selectingFirst = true;
            while (true)
            {
                Console.Clear();
                if (selectingFirst)
                    Console.WriteLine($"> {text1} {index1}\n{text2} {index2}");
                else
                    Console.WriteLine($"{text1} {index1}\n> {text2} {index2}");

                Console.WriteLine($"Use Arrows to change and press Tab or Shift + Tab to select");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (selectingFirst)
                    {
                        index1 = (index1 + 1) % maxNum1;
                    }
                    else
                    {
                        index2 = (index2 + 1) % maxNum2;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (selectingFirst)
                    {
                        index1 = (index1 - 1 + maxNum1) % maxNum1;
                    }
                    else
                    {
                        index2 = (index2 - 1 + maxNum2) % maxNum2;
                    }
                }
                else if ((key.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift && key.Key == ConsoleKey.Tab)
                {
                    selectingFirst = true;
                }
                else if (key.Key == ConsoleKey.Tab)
                {
                    if (selectingFirst)
                    {
                        selectingFirst = false;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return (index1, index2);
        }

        public static void RunEditor(Grid<char> gridList)
        {

            int gridRows = gridList.RowCount();
            int gridColumns = gridList.ColumnCount();

            int x = 1;
            int y = 1;

            ClearScreen();

            DrawGrid(gridList, x, y);

            int oldCursorLeft = Console.CursorLeft;
            int oldCursorTop = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(y, x + 1);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (x <= 0) x = gridRows - 1;
                    else x = Math.Max(0, x - 1);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (x >= gridRows - 1) x = 0;
                    else x = Math.Min(gridRows - 1, x + 1);
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (y <= 0) y = gridColumns - 1;
                    else y = Math.Max(0, y - 1);
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (y >= gridColumns - 1) y = 0;
                    else y = Math.Min(gridColumns - 1, y + 1);
                }
                else if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (gridList.GetValue(x, y) != ' ')
                    {
                        gridList.SetValue(x, y, ' ');
                    }
                    else
                    {
                        gridList.SetValue(x, y, '*');
                    }

                }
                else if (keyInfo.Key == ConsoleKey.Tab)
                {

                    char[] fields = new[] { '0', '1', '2', '3' };

                    int currentIndex = 0;
                    int totalFields = fields.Length;

                    while (true)
                    {
                        ClearScreen();

                        Console.WriteLine(fields[currentIndex]);

                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.UpArrow)
                        {
                            currentIndex = (currentIndex + 1) % totalFields;
                        }
                        else if (key.Key == ConsoleKey.DownArrow)
                        {
                            currentIndex = (currentIndex - 1 + totalFields) % totalFields;
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                    }
                    gridList.SetValue(x, y, fields[currentIndex]);
                }
                else if (keyInfo.Key == ConsoleKey.L)
                {
                    Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
                    string fileName = SelectFile();
                    Grid<char> temp = LoadFromFile(fileName);

                    gridRows = temp.RowCount();
                    gridColumns = temp.ColumnCount();
                    gridList = PopulateEmptyGrid(gridRows, gridColumns);
                    gridList = temp;
                }
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
                    SaveToFile(gridList);
                }
                else if (keyInfo.Key == ConsoleKey.R)
                {
                    Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
                    string strColumns;
                    int columns;
                    do
                    {
                        Console.Write("Columns: ");
                        strColumns = Console.ReadLine();
                    } while (!int.TryParse(strColumns, out columns));

                    string strRows;
                    int rows;
                    do
                    {
                        Console.Write("Rows: ");
                        strRows = Console.ReadLine();
                    } while (!int.TryParse(strRows, out rows));

                    oldCursorLeft += Math.Min(0, Math.Abs(columns - gridColumns));
                    oldCursorTop += Math.Max(0, rows - gridRows);

                    gridRows = rows;
                    gridColumns = columns;
                    gridList = ResizeGridList(gridList, rows, columns);
                }
                else if (keyInfo.Key == ConsoleKey.Escape) 
                {
                    Config.Init();
                    return;
                }

                ClearScreen();
                DrawGrid(gridList, x, y);
            }
        }

        private static Grid<char> ResizeGridList(Grid<char> original, int rows, int cols)
        {
            Grid<char> newList = new CharGrid(rows, cols);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (row == 0 || row > rows - 2 || col == 0 || col > cols - 2)
                    {
                        newList.SetValue(row, col, '*');
                    }
                    else
                    {
                        newList.SetValue(row, col, ' ');
                    }
                }
            }

            int minRows = Math.Min(rows, original.RowCount());
            int minCols = Math.Min(cols, original.ColumnCount());
            Debug.WriteLine($"mr = {minRows}, mc = {minCols}");
            for (int i = 0; i < minRows; i++)
            {
                for (int j = 0; j < minCols; j++)
                {
                    if (newList.GetValue(i, j) != '*') newList.SetValue(i, j, original.GetValue(i, j));
                }

            }

            return newList;
        }

        private static void ClearScreen()
        {
            // Clears the screen and the scrollback buffer in xterm-compatible terminals.
            Console.Clear(); Console.WriteLine("\x1b[3J");
        }

        public static Grid<char> PopulateEmptyGrid(int gridRows, int gridColumns)
        {
            return Load.PopulateEmptyGrid(gridRows, gridColumns);
        }

        static void DrawGrid(Grid<char> gridList, int x, int y)
        {
            string toPrint = "";
            for (int row = 0; row < gridList.RowCount(); row++)
            {
                for (int col = 0; col < gridList.ColumnCount(); col++)
                {
                    toPrint += gridList.GetValue(row, col);
                }
                toPrint += "\n";
            }
            Console.WriteLine(toPrint);
            Console.WriteLine("Move with arrow keys");
            Console.WriteLine("Spacebar = add wall, Tab to select char");
            Console.WriteLine("L = Load from file, S = Save to file, R = Resize Grid");
        }

        static void SaveToFile(Grid<char> grid)
        {
            Load.SaveToFile(grid);
        }

        public static string[] GetJsonFiles()
        {
            return Load.GetJsonFiles();
        }

        static string SelectFile()
        {
            string[] allJsonFIles = GetJsonFiles();

            int numberOfFiles = allJsonFIles.Length;
            int index = 0;

            while (true)
            {
                ClearScreen();

                Console.WriteLine($"File > {Path.GetFileName(allJsonFIles[index]).Replace(".json", "")}");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.UpArrow)
                {
                    index = (index - 1 + numberOfFiles) % numberOfFiles;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    index = (index + 1) % numberOfFiles;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }

            return allJsonFIles[index];
        }

        public static Grid<char> LoadFromFile(string fileName)
        {
            return Load.LoadFromFile(fileName);
        }
    }
}

