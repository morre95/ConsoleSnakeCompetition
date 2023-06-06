namespace ConsoleSnakeCompetition.Classes.Player
{
    public class TopScoreBoard : ScoreBoard
    {
        public int NumberOf
        {
            get; set;
        }
        public TopScoreBoard(params PlayerScore[] scores) : base(scores)
        {
            NumberOf = 10;
        }

        public override List<PlayerScore> GetLeaderboard()
        {
            return Repository.OrderByDescending(x => x.Score).Take(NumberOf).ToList();
        }

    }
}


