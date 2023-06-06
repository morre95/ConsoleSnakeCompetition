namespace ConsoleSnakeCompetition.Classes.Player
{
    public class PlayerScore
    {
        public string PlayerName { get; set; } = "";
        public int Score
        {
            get; set;
        }

        public DateTime Date
        {
            get; set;
        }

        public PlayerScore(string name, int score, DateTime when)
        {
            PlayerName = name;
            Score = score;
            Date = when;
        }
    }
}


