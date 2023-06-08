using System.Xml.Linq;

namespace ConsoleSnakeCompetition.Utilities.Logging
{

    public class Logger<TClass>
    {
        private readonly Type _className;

        private readonly string executionPath = Path.GetFullPath("Resources/Logging/");

        public static Logger<TClass> Instance => new Logger<TClass>();

        public string ClassName => _className.FullName;

        public bool ConsoleOutput { get; set; }

        public Logger()
        {
            _className = typeof(TClass);
            ConsoleOutput = false;
        }

        public void Trace(string message, params object[] args)
        {
            Log(message, LoggerColor.Trace, ClassName, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log(message, LoggerColor.Warn, ClassName, args);
        }

        public void Debug(string message, params object[] args)
        {
            Log(message, LoggerColor.Debug, ClassName, args);
        }

        public void Success(string message, params object[] args)
        {
            Log(message, LoggerColor.Success, ClassName, args);
        }

        public void Error(string message, params object[] args)
        {
            Log(message, LoggerColor.Error, ClassName, args);
        }

        public void Error(Exception e)
        {
            Log("An error occurred: " + Environment.NewLine + e, LoggerColor.Error, ClassName);
        }

        public void Log(string message, LoggerColor color, string className, params object[] args)
        {
            if (args.Length > 0) message += $" {string.Join(" ", args.Cast<object>().ToArray())}";

            if (ConsoleOutput) Output.WriteLine((ConsoleColor)color, $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
            var logFile = GetLogFile(color);

            LogToFile(logFile, $"Occurred at [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] in [{className}]: {message}");
        }

        private string GetLogFile(LoggerColor color)
        {
            if (color == LoggerColor.Warn) return "warn.log";
            if (color == LoggerColor.Debug) return "debug.log";
            if (color == LoggerColor.Error) return "error.log";
            if (color == LoggerColor.Success) return "success.log";
            if (color == LoggerColor.Trace) return "trace.log";
            else return "error.log";
        }

        private void LogToFile(string file, string content)
        {
            using (var fileWriter = new StreamWriter(executionPath + file, true))
            {
                fileWriter.WriteLine(content);
                fileWriter.Close();
            }
        }
    }
}


