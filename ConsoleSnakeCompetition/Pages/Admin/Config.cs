using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Menu;
using ConsoleSnakeCompetition.Pages.GamePlay;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.Admin
{
    internal class Config
    {

        public static void Init()
        {
            Console.Clear();
            // TBD: Kolla om det går att ta reda på vart Menu callas ifrån
            // *: Om det går kalla du kan skicka tillbaka användaren hit vid fel index
            var menu = new Menu(
                new Option("Home", Game.Init),
                new Option("Game Speed", SetSpeed),
                new Option("Best of", SetBestOf),
                new Option("Theme Color", SelectThemeColor),
                new Option("Editor", GridEditor.Init)
                );
            menu.SetPosition(0, 0);
            menu.Display();
        }

        public static void SelectThemeColor()
        {
            ConsoleColor[] colors = (ConsoleColor[]) ConsoleColor.GetValues(typeof(ConsoleColor));

            //int currentIndex = 0;
            ConsoleColor consoleColor;

            if (!Enum.TryParse(AppSettings.Instance.ThemeColor, out consoleColor))
            {
                Console.WriteLine("Invalid color, try again!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            int currentIndex = Array.IndexOf(colors, consoleColor);
            bool colorSelected = false;

            ConsoleColor currentBackground = Console.BackgroundColor;
            //ConsoleColor originalColor = Console.ForegroundColor;

            while (!colorSelected)
            {
                

                Console.Clear();

                Output.WriteLine(colors[currentIndex], $"Current color: {colors[currentIndex]}");
                Console.WriteLine("Select a color using the arrow keys (left/right) and press Enter to select.");

                ConsoleKeyInfo keyInfo = Console.ReadKey();


                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        currentIndex = (currentIndex - 1 + colors.Length) % colors.Length;
                        if (colors[currentIndex] == currentBackground)
                        {
                            // To jump over index 0, That is black, the same as background
                            currentIndex += currentIndex <= 0 ? colors.Length - 1 : -1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
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

            Output.WriteLine(colors[currentIndex], $"You Selected: {colors[currentIndex]}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();

            AppSettings.Instance.ThemeColor = Enum.GetName(typeof(ConsoleColor), colors[currentIndex]);
            AppSettings.Instance.SaveSettings();

            Init();
        }

        private static void SetBestOf()
        {
            int[] steps = { 3, 5, 7, 0 };
            // FIXME: bestOf verkar inte skrivas över när man startar spelet. Men den gör det dock när man ändrar och gårt tillbaka hit
            int currentIndex = Array.IndexOf(steps, AppSettings.Instance.BestOf);

            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Nuvarande värde: " + steps[currentIndex]);
                Console.WriteLine("Select with arrow key and enter to select");

                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                {
                    currentIndex = (currentIndex + 1) % steps.Length;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    currentIndex = (currentIndex - 1 + steps.Length) % steps.Length;
                }
                else if(key == ConsoleKey.Enter)
                {
                    break;
                }
            }

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

            AppSettings.Instance.Speed = selectedValue;

            AppSettings.Instance.SaveSettings();
            Init();
        }

    }
}

