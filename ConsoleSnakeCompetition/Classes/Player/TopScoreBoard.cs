using ConsoleSnakeCompetition.Utilities;
using static System.Formats.Asn1.AsnWriter;

namespace ConsoleSnakeCompetition.Classes.Player
{
    public class TopScoreBoard : ScoreBoard
    {
        public int NumberOfScores { get; set; } = 10;

        public TopScoreBoard() : base()
        {
            NumberOfScores = 10;
        }

        public TopScoreBoard(params PlayerScore[] scores) : base(scores)
        {
            NumberOfScores = 10;
        }

        public override List<PlayerScore> GetLeaderboard()
        {
            return Repository.OrderByDescending(x => x.Score).Take(NumberOfScores).ToList();
        }

        public override bool IsHighScoreWorthy(PlayerScore score)
        {
            if (Repository.Count < NumberOfScores)
            {
                return true;
            }

            foreach (PlayerScore oldScore in GetLeaderboard())
            {
                if (score.Score > oldScore.Score) return true;
            }

            return false;
        }

        public override void Add(PlayerScore score)
        {
            if (IsHighScoreWorthy(score))
            {
                if (Repository.Count < NumberOfScores)
                {
                    base.Add(score);
                }   
                else
                {
                    RemoveLowestScore();
                    Add(score);
                }
            }
             
        }

    }
}


