namespace ConsoleSnakeCompetition.Utilities;

public interface ILogger
{
    void Debug(string message, bool log = false);
    void Error(Exception e);
    void Error(string message, bool log = false);
    void Success(string message, bool log = false);
    void Trace(string message, bool log = false);
    void Warn(string message, bool log = false);
}