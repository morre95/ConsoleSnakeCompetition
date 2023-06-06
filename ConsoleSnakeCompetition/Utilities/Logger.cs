namespace ConsoleSnakeCompetition.Utilities
{
    public class Logger<TClass> : ILogger
    {
        private readonly Type _className;

        private static readonly string executionPath = Path.GetFullPath("Resources/Logging/");

        private static readonly Lazy<Logger<TClass>> instance = new Lazy<Logger<TClass>>(() => new Logger<TClass>());

        public static Logger<TClass> Instance => instance.Value;

        private Logger()
        {
            _className = typeof(TClass);
        }

        public void Trace(string message, bool log = false)
        {
            Log(message, ConsoleColor.White, log);
        }

        public void Warn(string message, bool log = false)
        {
            Log(message, ConsoleColor.Yellow, log);
        }

        public void Debug(string message, bool log = false)
        {
            Log(message, ConsoleColor.Cyan, log);
        }

        public void Success(string message, bool log = false)
        {
            Log(message, ConsoleColor.Green, log);
        }

        public void Error(string message, bool log = false)
        {
            Log(message, ConsoleColor.Red, log);
        }

        public void Error(Exception e)
        {
            Log("An error occurred: " + Environment.NewLine + e, ConsoleColor.Red);
        }

        private void Log(string message, ConsoleColor color, bool log = false)
        {
            Output.WriteLine(color, $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
            var logFile = GetLogFile(color);

            LogToFile(logFile, $"Occurred at [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] in [{_className.FullName}]: " + message);
        }

        private static string GetLogFile(ConsoleColor color)
        {
            string logFile;

            switch (color)
            {
                case ConsoleColor.Yellow:
                    logFile = "error.log";
                    break;
                case ConsoleColor.Cyan:
                    logFile = "debug.log";
                    break;
                case ConsoleColor.Red:
                    logFile = "error.log";
                    break;
                default:
                    logFile = "trace.log";
                    break;
            }

            return logFile;
        }

        private static void LogToFile(string file, string content)
        {
            using (var fileWriter = new StreamWriter(executionPath + file, true))
            {
                fileWriter.WriteLine(content);
                fileWriter.Close();
            }
        }
    }
}


