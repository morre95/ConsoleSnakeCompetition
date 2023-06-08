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
    }
}

