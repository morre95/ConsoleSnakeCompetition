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
        // TODO: Gör det möjligt så att ändra i inställningarna vad som händer om man försöker äta sig själv. Hint kolla efter if (snake.IsEatingPart(Snake.Direction.Up)) ReduceLength(1); Game.cs
        // TBD: Kolla om det går att använda GetXYAhead(bool) till något
        // TODO: Lägg till så att computern går snabbare och snabbare vid oändligt många ronder och kanske också vid de andra bestOf valen
        // TODO: Lägg till svårighetsgrader tex. easy, normal, hard
        // TODO: Skapa banor som finns vid installation, kan vara kopplade till svårighets nivåerna
        // TODO: Med olika banor skulle man kunna stiga i svårighetsgrad
        // TODO: Kanske xp kan vara något att införa

        // TBD:    Nedanstående
        // TODO: Gör spelet multiplayer, player 1 mot player 2, kanske till och med online alt. lokalt nätverk eller båda
        // TODO: Gör så man kan samarbeta mot datorn, kanske till och med online alt. lokalt nätverk eller båda
        static void Main(string[] args)
        {
            Setup.Init();

            AppSettings.Instance.LoadSettings();

            Game.Init();
        }  
    }
}


