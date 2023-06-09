using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleSnakeCompetition.Utilities;
using ConsoleSnakeCompetition.Utilities.Logging;

namespace ConsoleSnakeCompetition.Classes.Player
{
    public abstract class ScoreBoard
    {
        public List<PlayerScore> Repository { get; set; } = new();

        private readonly string ScoreBoardPath = Path.GetFullPath("Resources/Scores/");

        public ScoreBoard()
        {
            Repository = new List<PlayerScore>();
        }

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

        public virtual bool IsHighScoreWorthy(PlayerScore score)
        {
            foreach (PlayerScore oldScore in GetLeaderboard())
            {
                if (score.Score > oldScore.Score) return true;
            }

            return false;
        }

        public virtual void RemoveLowestScore()
        {
            if (Repository.Count <= 0)
            {
                return;
            }

            // Hitta den lägsta poängen
            int lowestScore = GetLeaderboard()[0].Score;
            foreach (PlayerScore score in Repository)
            {
                if (score.Score < lowestScore)
                {
                    lowestScore = score.Score;
                }
            }

            // Ta bort det första objektet med lägsta poängen
            for (int i = 0; i < Repository.Count; i++)
            {
                if (Repository[i].Score == lowestScore)
                {
                    Repository.RemoveAt(i);
                    break;
                }
            }
        }

        public virtual void Save()
        {
            var fileName = "scores.json";
            SaveToFile(ScoreBoardPath + fileName);
        }

        public virtual void Load()
        {
            var fileName = "scores.json";
            LoadFromFile(ScoreBoardPath + fileName);
        }

        public virtual void SaveToFile(string fileName)
        {
            var jsonString = JsonSerializer.Serialize(Repository);
            File.WriteAllText(fileName, jsonString);
        }

        public virtual void LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    var jsonString = File.ReadAllText(fileName);
                    Repository = JsonSerializer.Deserialize<List<PlayerScore>>(jsonString)!;
                }
                catch (JsonException ex)
                {
                    Log.Error(ex);
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(ex);
                }
            }
            else
            {
                Log.Error($"File don't exists: {fileName}");
            }
        }

        public override string ToString()
        {
            var tb = new TableBuilder();
            tb.AddRow("#",  "Name",     "Score", "When");
            tb.AddRow("--", "--------", "-----", "-------------------");
            var i = 1;
            foreach (var score in GetLeaderboard())
            {
                tb.AddRow(i++, score.PlayerName, score.Score.ToString(), score.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return tb.Output();
        }
    }
}


