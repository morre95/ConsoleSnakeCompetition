using System.Text.Json;
using ConsoleSnakeCompetition.Utilities;

namespace ConsoleSnakeCompetition.Classes.Player
{
    public abstract class ScoreBoard
    {
        public List<PlayerScore> Repository { get; private set; } = new();

        private readonly string ScoreBoardPath = Path.GetFullPath("Resources/Scores/");

        public ScoreBoard(params PlayerScore[] scores)
        {
            Repository = new List<PlayerScore>(scores);
        }

        public virtual void Add(PlayerScore score)
        {
            Repository.Add(score);
        }

        public virtual void AddRange(params PlayerScore[] scores)
        {
            Repository.AddRange(scores);
        }

        public virtual List<PlayerScore> GetLeaderboardAsc()
        {
            return Repository.OrderBy(x => x.Score).ToList();
        }

        public virtual List<PlayerScore> GetLeaderboard()
        {
            return Repository.OrderByDescending(x => x.Score).ToList();
        }

        public virtual void Save()
        {
            var fileName = "scores.json";
            SaveToFile(ScoreBoardPath + fileName);
        }

        public virtual void SaveToFile(string fileName)
        {
            var jsonString = JsonSerializer.Serialize(Repository);
            File.WriteAllText(fileName, jsonString);
        }

        public virtual void LoadFromFile(string fileName)
        {
            var jsonString = File.ReadAllText(fileName);
            Repository = JsonSerializer.Deserialize<List<PlayerScore>>(jsonString)!;
        }

        public override string ToString()
        {
            var tb = new TableBuilder();
            tb.AddRow("#", "Name", "Score", "When");
            tb.AddRow("--", "----", "-----", "-----------");
            var i = 1;
            foreach (var score in GetLeaderboard())
            {
                tb.AddRow(i++, score.PlayerName, score.Score.ToString(), score.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return tb.Output();
        }
    }
}


