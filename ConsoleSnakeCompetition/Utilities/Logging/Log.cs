using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConsoleSnakeCompetition.Utilities.Logging
{

    public static class Log
    {
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
            // Implementera loggningen och användande callingClassName, level, message, args
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

    /*public static class Log
    {
        public static void Trace(string message, params object[] args)
        {
            Logger<Program>.Instance.Trace(message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            Logger<Program>.Instance.Warn(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            Logger<Program>.Instance.Debug(message, args);
        }

        public static void Success(string message, params object[] args)
        {
            Logger<Program>.Instance.Success(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Logger<Program>.Instance.Error(message, args);
        }

        public static void Error(Exception e)
        {
            Logger<Program>.Instance.Error(e);
        }

    }*/
}


