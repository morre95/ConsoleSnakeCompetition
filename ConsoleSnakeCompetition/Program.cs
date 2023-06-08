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
        
        // TODO: lägg till highscore
        // TODO: Lägg till så att computern går snabbare och snabbare vid oändligt många ronder och kanske också vid de andra bestOf valen

        // TBD:    Nedanstående
        // TODO: Gör spelet multiplayer
        // TODO: Gör så man kan samarbeta mot datorn
        static void Main(string[] args)
        {
            Setup.Init();

            AppSettings.Instance.LoadSettings();

            Game.Init();
        }  
    }
}


