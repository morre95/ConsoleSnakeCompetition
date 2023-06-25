using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class GameTournament
    {
        public static void Run()
        {
            Console.Clear();
            var round = 1;

            var players = CreatePlayers();

            // INFO: använd Game2P.cs Run metoden för omgångarna, ändra om det behövs i den
            // INFO: Det är förberett så man kan använda AppSettings.Instance.Player1Name och .Player1Name för att visa rätt namn i runderna
            // INFO: Använd tex en json fil för att spara bracketen för turneringen
            // INFO: 
            while (players.Count > 1)
            {
                var matchups = CreateMatchups(players);
                PrintBracket(matchups, round++);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                players = PlayRound(matchups);
            }

            if (players.Count == 1)
            {
                Console.WriteLine("\n" + players.Single().Name + " is the winner!");
            }

        }

        private static List<Player> CreatePlayers()
        {
            //int playerCount = 7;
            //return Enumerable.Range(1, playerCount).Select(x => new Player($"Player {x}", 0)).ToList();

            int playerCount = 0;
            Console.Write("Number of players: ");
            while (!int.TryParse(Console.ReadLine(), out playerCount))
            {
                Console.WriteLine("Not a number. Try again");
            }
            
            var players = new List<Player>();

            for (int i = 0; i < playerCount; i++)
            {
                Console.Write($"Player {i + 1} name: ");
                string player = Console.ReadLine();
                players.Add(new Player(player, 0));
            }

            return players;
        }

        private static List<Matchup> CreateMatchups(List<Player> players)
        {
            var matchups = new List<Matchup>();

            for (int i = 0; i < players.Count / 2; i++)
            {
                matchups.Add(new Matchup(players[i], players[players.Count - (i + 1)]));
            }

            if (players.Count % 2 != 0)
            {
                var missingPlayer = players[players.Count / 2];
                matchups.Add(new Matchup(missingPlayer, null));
            }

            return matchups;
        }

        private static void PrintBracket(List<Matchup> Matchups, int round)
        {
            Output.WriteLine(ConsoleColor.Blue, "\nRound " + round + ":");
            foreach (var matchup in Matchups.OrderBy(x => x.GetFavored().Points))
            {
                Console.WriteLine(matchup);
            }
        }

        private static List<Player> PlayRound(List<Matchup> matchups)
        {
            var players = new List<Player>();

            foreach (var matchup in matchups)
            {
                if (matchup.PlayerB == null)
                {
                    players.Add(matchup.PlayerA);
                    continue;
                }
                /*Console.Write($"\nWinner? {matchup.PlayerA.Name} = 1, {matchup.PlayerB.Name} = 2 ");
                string input;
                int winner = 0;

                while (winner != 1 && winner != 2)
                {
                    input = Console.ReadLine();

                    if (int.TryParse(input, out winner))
                    {
                        if (winner != 1 && winner != 2)
                        {
                            Console.WriteLine($"{input} is invalid. Try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"'{input}' is invalid input. Try again.");
                    }
                }

                if (winner == 1)
                {
                    matchup.PlayerA.Points++;
                    players.Add(matchup.PlayerA);
                }
                else
                {
                    matchup.PlayerB.Points++;
                    players.Add(matchup.PlayerB);
                }*/


                // FIXME: Behöver provköras. Något galet när turnerings resultatet visas. Svårt att förstå eller helt fel
                Console.WriteLine($"{matchup.PlayerA.Name} vs {matchup.PlayerB.Name}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

                AppSettings.Instance.Player1Name = matchup.PlayerA.Name;
                AppSettings.Instance.Player2Name = matchup.PlayerB.Name;

                string winner = Game2P.Run();
                if (winner == matchup.PlayerA.Name) 
                {
                    matchup.PlayerA.Points++;
                    players.Add(matchup.PlayerA);
                } 
                else if (winner == matchup.PlayerB.Name)
                {
                    matchup.PlayerB.Points++;
                    players.Add(matchup.PlayerB);
                }
                else if (winner == null)
                {
                    Console.WriteLine("It was a draw");
                    Console.Write("Play again? y = yes, n = no ");
                    string yesOrNo = Console.ReadLine();
                    while (yesOrNo != "y" && yesOrNo != "n")
                    {
                        Console.WriteLine($"'{yesOrNo}' is not valid. Try again");
                        yesOrNo = Console.ReadLine();
                    }

                    Console.ReadKey(true);
                }
               

            }

            return players;
        }
    }

    public class Player
    {
        public Player(string name, int point)
        {
            Name = name;
            Points = point;
        }

        public string Name
        {
            get; set;
        }

        public int Points
        {
            get; set;
        }

        public static Player operator <(Player pA, Player pB)
        {
            if (pB == null) return pA;
            return pA.Points < pB.Points ? pA : pB;
        }

        public static Player operator >(Player pA, Player pB)
        {
            if (pB == null) return pA;
            return pA.Points > pB.Points ? pA : pB;
        }
    }

    public class Matchup
    {
        public Matchup(Player pA, Player pB = null)
        {
            PlayerA = pA;
            PlayerB = pB;
        }

        public Player PlayerA
        {
            get; set;
        }
        public Player PlayerB
        {
            get; set;
        }

        public override string ToString()
        {
            return PlayerB == null ? $"{PlayerA.Name} has a free run" : $"{PlayerA.Name} vs. {PlayerB.Name}";
        }

        public Player GetFavored()
        {
            return PlayerA > PlayerB;
        }
    }
}
