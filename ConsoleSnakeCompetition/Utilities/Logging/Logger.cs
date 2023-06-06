using System.Xml.Linq;

namespace ConsoleSnakeCompetition.Utilities.Logging
{
    public class Logger<TClass>
    {
        private readonly Type _className;

        private static readonly string executionPath = Path.GetFullPath("Resources/Logging/");

        private static readonly Lazy<Logger<TClass>> instance = new Lazy<Logger<TClass>>(() => new Logger<TClass>());

        public static Logger<TClass> Instance => instance.Value;

        private Logger()
        {
            _className = typeof(TClass);
        }

        public void Trace(string message, params object[] args)
        {
            Log(message, ConsoleColor.White, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log(message, ConsoleColor.Yellow, args);
        }

        public void Debug(string message, params object[] args)
        {
            Log(message, ConsoleColor.Cyan, args);
        }

        public void Success(string message, params object[] args)
        {
            Log(message, ConsoleColor.Green, args);
        }

        public void Error(string message, params object[] args)
        {
            Log(message, ConsoleColor.Red, args);
        }

        public void Error(Exception e)
        {
            Log("An error occurred: " + Environment.NewLine + e, ConsoleColor.Red);
        }

        public void Fatal(string message, params object[] args)
        {
            Log(message, ConsoleColor.Red, args);
        }

        private void Log(string message, ConsoleColor color, params object[] args)
        {
            if (args.Length > 0) message += $" {string.Join(" ", args.Cast<object>().ToArray())}";

            Output.WriteLine(color, $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
            var logFile = GetLogFile(color);

            LogToFile(logFile, $"Occurred at [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] in [{_className.FullName}]: {message}");
        }

        private static string GetLogFile(ConsoleColor color)
        {
            if (color == ConsoleColor.Yellow) return "error.log";
            if (color == ConsoleColor.Cyan) return "debug.log";
            if (color == ConsoleColor.Red) return "error.log";
            else return "trace.log";
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


