using System.Runtime.CompilerServices;

namespace ConsoleSnakeCompetition.Utilities.Logging
{
    public static class Log
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

        public static void Fatal(string message, params object[] args)
        {
            Logger<Program>.Instance.Fatal(message, args);
        }
        
        public static void Test(string message, [CallerMemberName] string memberName = null, params object[] args)
        {
            Logger<Program>.Instance.Success(memberName, args);
        }
    }
}


