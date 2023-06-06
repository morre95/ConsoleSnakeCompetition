namespace ConsoleSnakeCompetition.Utilities.Logging
{
    public static class Log
    {
        public static void Trace(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Trace(message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Warn(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Debug(message, args);
        }

        public static void Success(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Success(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Error(message, args);
        }

        public static void Error(Exception e)
        {
            Logger<AppSettings>.Instance.Error(e);
        }

        public static void Fatal(string message, params object[] args)
        {
            Logger<AppSettings>.Instance.Fatal(message, args);
        }
    }
}


