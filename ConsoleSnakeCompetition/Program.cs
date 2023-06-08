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

using ConsoleSnakeCompetition.Pages.Admin;
using System;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Grid<char> gridList = GridEditor.PopulateEmptyGrid(22, 88);
            //GridEditor.RunEditor(gridList);

            Tests();
            return;

            // TBD: Skapandet av katalog struktur här bör flyttas ut tull en Setup klass
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

        
        // TBD: Bör raderas
        private void testProp()
        {
            // INFO: Gör detta till något som spånar in samtidigt som ny mat för att göra spelet med utmanande
            // TBD: Låt dator masken prioritera dessa chars före maten
            // TBD: w = automatisk vinnst, 2, 3 och 4 = antal ökning i längd 
            List<KeyValuePair<double, char>> probabilitieChars = new List<KeyValuePair<double, char>>();
            probabilitieChars.Add(new KeyValuePair<double, char>(1, 'w'));
            probabilitieChars.Add(new KeyValuePair<double, char>(50, '2'));
            probabilitieChars.Add(new KeyValuePair<double, char>(0.001, '3'));
            probabilitieChars.Add(new KeyValuePair<double, char>(30, '4'));

            Random rand = new Random();
            int wS = 0;
            int tows = 0;
            int threes = 0;
            int fours = 0;
            int times = 1000000;

            for (int i = 0; i < times; i++)
            {
                double randomValue = rand.NextDouble() * 100;

                foreach (KeyValuePair<double, char> pair in probabilitieChars)
                {
                    if (randomValue < pair.Key)
                    {
                        switch (pair.Value)
                        {
                            case 'w':
                                wS++;
                                break;
                            case '2':
                                tows++;
                                break;
                            case '3':
                                threes++;
                                break;
                            case '4':
                                fours++;
                                break;
                        }
                    }
                }
            }

            Console.WriteLine($"W = {(double)wS / times * 100}%, {wS} times");
            Console.WriteLine($"2 = {(double)tows / times * 100}%, {tows} time");
            Console.WriteLine($"3 = {(double)threes / times * 100}%, {threes} time");
            Console.WriteLine($"4 = {(double)fours / times * 100}%, {fours} time");
        }

        // TBD: Bör radderas
        // INFO: Test körning av Output klassen och Logger och Log klasserna
        private static void Tests()
        {
            Output.Write(ConsoleColor.Red, "sdfgh");
            Output.Write(ConsoleColor.Yellow, '#');
            Output.Write(ConsoleColor.Red, 12);
            Output.Write(ConsoleColor.Red, null);
            Output.Write(ConsoleColor.Magenta, false);
            Output.Write(ConsoleColor.DarkBlue, 12.55);
            Output.Write(ConsoleColor.Green, 22.5f);

            Output.WriteLine(ConsoleColor.Red, "sdfgh");
            Output.WriteLine(ConsoleColor.Blue, '#');
            Output.WriteLine(ConsoleColor.Yellow, 12);
            Output.WriteLine(ConsoleColor.Yellow, null);
            Output.WriteLine(ConsoleColor.Magenta, false);
            Output.WriteLine(ConsoleColor.Yellow, 12.55);
            Output.WriteLine(ConsoleColor.DarkBlue, 22.5f);

            Console.WriteLine("--------------------------");

            var logger = new Logger<Program>();
            logger.ConsoleOutput = true;
            logger.Warn("Värdet måste varnas");
            logger.Error("Error");

            var sLogger = new Logger<Snake>();
            sLogger.Trace("Trace the snake");
            new Logger<Menu>().Debug("Nu debuggar vi menu");
            new Logger<AppSettings>().Success("Yes det funkade");

            Logger<Menu>.Instance.Debug("Debug message");
            
            Log.Error("Error 123");
            Log.Debug("Debug 123", "Foo", "Bar", 1, 2, 'E');
            Log.ConsoleOutput = true;
            Log.Success("Debuga mig, och skriv ut");
            Log.Trace("Where is the snake, trace it if you find it");
        }
    }
}


