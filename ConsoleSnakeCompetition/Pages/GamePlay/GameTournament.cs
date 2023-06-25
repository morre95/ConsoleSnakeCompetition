using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class GameTournament
    {
        public static void Run()
        {
            var playerCount = 7;
            var round = 1;

            var players = CreatePlayers(playerCount);

            // INFO: använd Game2P.cs Run metoden för omgångerna, ändra om det behövs i den
            // INFO: Det är förberett så man kan använda AppSettings.Instance.Player1Name och .Player1Name för att visa rätt namn i runderna
            // INFO: Använd tex en json fil för att spara bracketen för turneringen
            // INFO: 
            while (players.Count > 1)
            {
                var matchups = CreateMatchups(players);
                PrintBracket(matchups, round++);
                players = PlayRound(matchups);
            }

            if (players.Count == 1)
            {
                Console.WriteLine("\n" + players.Single().Name + " is the winner!");
            }

        }

        private static List<Player> CreatePlayers(int totaPlayers)
        {
            return Enumerable.Range(1, totaPlayers).Select(x => new Player("Player " + x, 0)).ToList();
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
            Console.WriteLine("Round " + round + ":\n");
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
                Console.Write($"\nWinner? {matchup.PlayerA.Name} = 1, {matchup.PlayerB.Name} = 2 ");
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
