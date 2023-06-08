﻿using System;
using System.Collections.Generic;
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
                new Option("Rounds", Option.NIY),
                new Option("Snake Color", Option.NIY),
                new Option("Theme", Option.NIY),
                new Option("Editor", GridEditor.Init)
                );
            menu.SetPosition(0, 0);
            menu.Display();
        }
        private static void SetSpeed()
        {
            var stepCount = AppSettings.Instance.StepSpeedCount;
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
