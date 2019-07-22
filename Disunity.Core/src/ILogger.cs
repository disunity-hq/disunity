namespace Disunity.Core
{

    public interface ILogger
    {

        void Log(LogLevel level, string message);

        void LogDebug(string message);

        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

    }

}