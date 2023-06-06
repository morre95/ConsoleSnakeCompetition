using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Classes.Menu
{
    public class Menu
    {
        const int startX = 5;
        const int startY = 9;
        const int optionsPerLine = 3;
        const int spacingPerLine = 14;

        private List<Option> Options
        {
            get; set;
        }

        public Menu()
        {
            Options = new List<Option>();
        }

        public Menu(List<Option> options)
        {
            Options = options;
        }

        public Menu(params Option[] options)
        {
            Options = new List<Option>(options);
        }

        public void Display()
        {
            var choice = 0;

            ConsoleKey key;

            Console.CursorVisible = false;
            do
            {
                WriteMenu(choice);

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (choice % optionsPerLine > 0)
                            choice--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (choice % optionsPerLine < optionsPerLine - 1)
                            choice++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (choice >= optionsPerLine)
                            choice -= optionsPerLine;
                        break;
                    case ConsoleKey.DownArrow:
                        if (choice + optionsPerLine < Options.Count)
                            choice += optionsPerLine;
                        break;
                        
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            CallCallback(choice);
        }

        private void CallCallback(int choice)
        {
            if (choice >= 0 && choice < Options.Count)
            {
                Options[choice].Callback();
            }
            else
            {
                Console.WriteLine("\nInvalid selection. Please try again");
                Thread.Sleep(3000);
                Display();
            }
        }

        private void WriteMenu(int choice)
        {
            for (var i = 0; i < Options.Count; i++)
            {
                Console.SetCursorPosition(startX + i % optionsPerLine * spacingPerLine, startY + i / optionsPerLine);

                if (i == choice)
                {
                    Output.Write(ConsoleColor.Red, Options[i]);
                }
                else
                {
                    Console.Write(Options[i]);
                }
            }
        }

        public Menu Add(string option, Action callback)
        {
            Options.Add(new Option(option, callback));
            return this;
        }

    }
}


