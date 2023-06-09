using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Utilities.Logging;

namespace ConsoleSnakeCompetition
{
    internal class Setup
    {
        public static void Init()
        {
            CreateDirIfNotExists();
        }

        public static void CreateDirIfNotExists()
        {
            string directoryPath = Path.GetFullPath(@"Resources"); ;

            if (!Directory.Exists(directoryPath))
            {
                CreateDirectories(directoryPath);
                CreateFakeScores();
            }
        }

        private static void CreateDirectories(string directoryPath)
        {
            try
            {
                Directory.CreateDirectory(@$"{directoryPath}\Grids");
                Directory.CreateDirectory(@$"{directoryPath}\Logging");
                Directory.CreateDirectory(@$"{directoryPath}\Scores");
                Directory.CreateDirectory(@$"{directoryPath}\Settings");
                Log.Success("Installing Script");
            }
            catch (Exception ex)
            {
                Logger<Program>.Instance.Error(ex);
            }
        }

        private static void CreateFakeScores()
        {
            string scoreBoardPath = Path.GetFullPath(@"Resources\Scores\");
            string scoresJSON = @"[{""PlayerName"":""Unused Kong"",""Opponent"":"""",""Score"":999,""Date"":""2079-09-29T15:34:34""},{""PlayerName"":""HelplessNezete"",""Opponent"":"""",""Score"":42,""Date"":""2011-02-10T07:41:35""},{""PlayerName"":""Vicky Zombi"",""Opponent"":"""",""Score"":33,""Date"":""1994-05-16T10:45:08""},{""PlayerName"":""RoundAss"",""Opponent"":"""",""Score"":52,""Date"":""2021-05-01T07:36:30""},{""PlayerName"":""CaringTense"",""Opponent"":"""",""Score"":49,""Date"":""2023-01-05T05:14:30""},{""PlayerName"":""AutomaticEulah"",""Opponent"":"""",""Score"":59,""Date"":""2021-09-13T08:25:56""},{""PlayerName"":""OtherCold"",""Opponent"":"""",""Score"":69,""Date"":""2022-09-01T22:51:02""},{""PlayerName"":""ChuckBanger"",""Opponent"":"""",""Score"":129,""Date"":""2023-02-05T12:22:11""},{""PlayerName"":""JollyBeret"",""Opponent"":"""",""Score"":19,""Date"":""1998-08-10T19:16:13""},{""PlayerName"":""Gandalf"",""Opponent"":"""",""Score"":22,""Date"":""2022-02-05T12:22:11""},{""PlayerName"":""MasterFishNet"",""Opponent"":"""",""Score"":151,""Date"":""1975-01-22T22:09:01""}]";
            string fileName = scoreBoardPath + "scores.json";
            File.WriteAllText(fileName, scoresJSON);
        }
    }
}

