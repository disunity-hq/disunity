namespace Disunity.Core {

    public abstract class LoggerBase: ILogger {

        public abstract void Log(LogLevel level, string message);

        public void LogDebug(string message) {
            Log(LogLevel.Debug, message);
        }

        public void LogInfo(string message) {
            Log(LogLevel.Info, message);
        }

        public void LogWarning(string message) {
            Log(LogLevel.Warning, message);
        }

        public void LogError(string message) {
            Log(LogLevel.Error, message);
        }

    }

}