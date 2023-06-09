using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Menu;
using ConsoleSnakeCompetition.Classes.Player;
using ConsoleSnakeCompetition.Pages.GamePlay;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.Admin
{

    internal class Config
    {

        public static void Init()
        {
            Console.Clear();
            var menu = new Menu(
                new Option("Home", Game.Init),
                new Option("Game Speed", SetSpeed),
                new Option("Best of", SetBestOf),
                new Option("Theme Color", SelectThemeColor),
                new Option("Food Color", SelectFoodColor),
                new Option("Editor", GridEditor.Init),
                new Option("Remove scores", RemoveAllScores)
                );
            menu.SetPosition(0, 0);
            menu.Display();
        }

        public static void RemoveAllScores()
        {
            Console.Clear();
            if (UtilsConsole.Confirm("Remove all scores?"))
            {
                ScoreBoard score = new TopScoreBoard();
                score.Save();
                Console.WriteLine("All scores are removed");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            Init();
        }

        public static void SelectFoodColor()
        {
            var consoleColor = ParseColor(AppSettings.Instance.FoodColor);

            AppSettings.Instance.FoodColor = SelectColor(consoleColor);
            AppSettings.Instance.SaveSettings();

            Init();
        }

        private static ConsoleColor ParseColor(string color)
        {
            ConsoleColor consoleColor;

            if (!Enum.TryParse(color, out consoleColor))
            {
                Console.WriteLine("Invalid color, default color is used");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return Console.ForegroundColor;
            }

            return consoleColor;
        }

        public static void SelectThemeColor()
        {

            ConsoleColor consoleColor;

            if (!Enum.TryParse(AppSettings.Instance.ThemeColor, out consoleColor))
            {
                Console.WriteLine("Invalid color, try again!");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Init();
                return;
            }


            AppSettings.Instance.ThemeColor = SelectColor(consoleColor);
            AppSettings.Instance.SaveSettings();

            Init();
        }

        public static string SelectColor(ConsoleColor consoleColor)
        {
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            int currentIndex = Array.IndexOf(colors, consoleColor);
            bool colorSelected = false;

            ConsoleColor currentBackground = Console.BackgroundColor;

            while (!colorSelected)
            {


                Console.Clear();

                Output.WriteLine(colors[currentIndex], $"Current color: {colors[currentIndex]}");
                Console.WriteLine("Select a color using the arrow keys (left/right) and press Enter to select.");

                ConsoleKeyInfo keyInfo = Console.ReadKey();


                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.DownArrow:
                        currentIndex = (currentIndex - 1 + colors.Length) % colors.Length;
                        if (colors[currentIndex] == currentBackground)
                        {
                            // To jump over index 0, That is black, the same as background
                            currentIndex += currentIndex <= 0 ? colors.Length - 1 : -1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                        currentIndex = (currentIndex + 1) % colors.Length;
                        if (colors[currentIndex] == currentBackground)
                        {
                            currentIndex++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        colorSelected = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid key. Try again.");
                        break;
                }
            }

            Console.Clear();
            Output.WriteLine(colors[currentIndex], $"You Selected: {colors[currentIndex]}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            return Enum.GetName(typeof(ConsoleColor), colors[currentIndex]);
        }

        private static void SetBestOf()
        {
            int[] steps = { 1, 3, 5, 7, 9, 0 };
            int currentIndex = Array.IndexOf(steps, AppSettings.Instance.BestOf);
            bool indexSelected = false;

            Console.CursorVisible = false;
            while (!indexSelected)
            {
                Console.Clear();
                Console.WriteLine($"Current value: {steps[currentIndex]}");
                Console.WriteLine("Select with arrow key and enter to select. 0 = ~infinity");

                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.DownArrow:
                        currentIndex = (currentIndex - 1 + steps.Length) % steps.Length;
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.RightArrow:
                        currentIndex = (currentIndex + 1) % steps.Length;
                        break;
                    case ConsoleKey.Enter:
                        indexSelected = true;
                        break;

                }
            }

            Console.Clear();
            Console.WriteLine($"You Selected: {steps[currentIndex]}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            AppSettings.Instance.BestOf = steps[currentIndex];
            AppSettings.Instance.SaveSettings();
            Init();
        }

        private static void SetSpeed()
        {
            var stepCount = AppSettings.Instance.StepStepCount;
            var selectedValue = AppSettings.Instance.Speed;

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

            Console.Clear();
            Console.WriteLine($"You Selected: {selectedValue}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            AppSettings.Instance.Speed = selectedValue;

            AppSettings.Instance.SaveSettings();
            Init();
        }

    }
}

