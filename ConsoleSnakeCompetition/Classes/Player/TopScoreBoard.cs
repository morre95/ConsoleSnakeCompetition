namespace ConsoleSnakeCompetition.Classes.Player
{
    public class TopScoreBoard : ScoreBoard
    {
        public int NumberOfScores
        {
            get; set;
        }
        public TopScoreBoard(params PlayerScore[] scores) : base(scores)
        {
            NumberOfScores = 10;
        }

        public override List<PlayerScore> GetLeaderboard()
        {
            return Repository.OrderByDescending(x => x.Score).Take(NumberOfScores).ToList();
        }

    }
}


