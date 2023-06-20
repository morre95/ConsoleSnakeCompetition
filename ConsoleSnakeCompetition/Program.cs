using System.Drawing;
using ConsoleSnakeCompetition.Classes.Algorithms;
using ConsoleSnakeCompetition.Classes.Game;
using ConsoleSnakeCompetition.Classes.Snake;
using ConsoleSnakeCompetition.Pages.GamePlay;
using ConsoleSnakeCompetition.Utilities;
using ConsoleSnakeCompetition.Utilities.Logging;

namespace ConsoleSnakeCompetition
{
    internal class Program
    {
        // TODO: Lägg till svårighetsgrader tex. easy, normal, hard
        // TODO: Skapa banor som finns vid installation, kan vara kopplade till svårighets nivåerna
        // TODO: Med olika banor skulle man kunna stiga i svårighetsgrad
        // TODO: Kanske xp kan vara något att införa

        static void Main(string[] args)
        {
            Setup.Init();

            AppSettings.Instance.LoadSettings();

            Game.Init();
        }  
    }
}


