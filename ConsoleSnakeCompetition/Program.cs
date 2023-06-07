using System.Diagnostics;
using System.Drawing;
using System.Formats.Asn1;
using System.Text.Json;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Menu;
using ConsoleSnakeCompetition.Classes.Player;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Pages.GamePlay;
using ConsoleSnakeCompetition.Utilities;
using ConsoleSnakeCompetition.Utilities.Logging;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Tests();
            //return;

            string directoryPath = Path.GetFullPath(@"Resources"); ;

            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(@$"{directoryPath}\Grids");
                    Directory.CreateDirectory(@$"{directoryPath}\Logging");
                    Directory.CreateDirectory(@$"{directoryPath}\Scores");
                    Directory.CreateDirectory(@$"{directoryPath}\Settings");
                    Log.Success("Installing Script");
                }
                catch (Exception ex)
                {
                    Logger<Program>.Instance.Error(ex);
                }
            }


            AppSettings.Instance.LoadSettings();

            Game.Init();
        }

        private static void Tests()
        {
            Output.WriteLine(ConsoleColor.Red, "sdfgh");
            Output.WriteLine(ConsoleColor.Red, '#');
            Output.WriteLine(ConsoleColor.Red, 12);
            Output.WriteLine(ConsoleColor.Red, null);
            Output.WriteLine(ConsoleColor.Red, false);
            Output.WriteLine(ConsoleColor.Red, 12.55);
            Output.WriteLine(ConsoleColor.Red, 22.5f);


            Output.Write(ConsoleColor.Red, "sdfgh");
            Output.Write(ConsoleColor.Red, '#');
            Output.Write(ConsoleColor.Red, 12);
            Output.Write(ConsoleColor.Red, null);
            Output.Write(ConsoleColor.Red, false);
            Output.Write(ConsoleColor.Red, 12.55);
            Output.Write(ConsoleColor.Red, 22.5f);

            var logger = new Logger<Program>();
            logger.Warn("Värdet måste varnas");
            logger.Error("Error");

            var sLogger = new Logger<Snake>();
            sLogger.Trace("Trace the snake");
            new Logger<Snake>().Debug("Nu debuggar vi menu");
            new Logger<AppSettings>().Success("Yes det funkade");

            Logger<Menu>.Instance.Debug("Debug message");

            Log.Error("Error");
            Log.Debug("Debug", "Foo", "Bar", 1, 2, 'E');
            Log.Success("Debuga mig");
        }
    }
}


