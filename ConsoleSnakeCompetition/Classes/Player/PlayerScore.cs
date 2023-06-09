using System.Text.Json.Serialization;

namespace ConsoleSnakeCompetition.Classes.Player
{
    public class PlayerScore
    {
        public string PlayerName { get; set; }
        public string Opponent { get; set; }
        public int Score { get; set; }

        public DateTime Date { get; set; }

        public PlayerScore() { } // JsonSerializer.Deserialize require empty constructor

        public PlayerScore(string name, int score, DateTime when, string opponent = "")
        {
            PlayerName = name;
            Score = score;
            Date = when;
            Opponent = opponent;
        }
    }
}


