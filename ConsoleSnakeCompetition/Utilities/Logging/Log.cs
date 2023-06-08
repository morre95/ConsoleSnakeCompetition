using System.Diagnostics;
using System.Reflection;

namespace ConsoleSnakeCompetition.Utilities.Logging
{

    public static class Log
    {
        private static bool _consoleOutput = false;
        public static bool ConsoleOutput
        {
            get => _consoleOutput;
            set => _consoleOutput = value;
        }

        public static void Trace(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "TRACE", message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "WARN", message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "DEBUG", message, args);
        }

        public static void Success(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "SUCCESS", message, args);
        }

        public static void Error(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "ERROR", message, args);
        }

        public static void Error(Exception e)
        {
            LogInternal(GetCallingClassName(), "ERROR", e.ToString());
        }

        public static void Fatal(string message, params object[] args)
        {
            LogInternal(GetCallingClassName(), "FATAL", message, args);
        }

        private static void LogInternal(string callingClassName, string level, string message, params object[] args)
        {
            var dict = new Dictionary<string, LoggerColor> {
                { "TRACE", LoggerColor.Trace },
                { "WARN", LoggerColor.Warn },
                { "DEBUG", LoggerColor.Debug },
                { "SUCCESS", LoggerColor.Success },
                { "ERROR", LoggerColor.Error }
            };
            var instance = Logger<Program>.Instance;
            instance.ConsoleOutput = _consoleOutput;
            instance.Log(message, dict[level], callingClassName, args);
        }

        private static string GetCallingClassName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame callingFrame = stackTrace.GetFrame(2);
            MethodBase callingMethod = callingFrame.GetMethod();
            //string callingClassName = callingMethod.DeclaringType.Name;
            string callingClassName = callingMethod.DeclaringType.Namespace;
            callingClassName += ".";
            callingClassName += callingMethod.DeclaringType.Name;
            return callingClassName;
        }
    }
}


