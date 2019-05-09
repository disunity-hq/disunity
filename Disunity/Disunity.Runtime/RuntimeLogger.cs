using BepInEx.Logging;

namespace Disunity.Runtime {
    public class RuntimeLogger {

        protected ManualLogSource _logger;
        public RuntimeLogger(string name) {
            _logger = Logger.CreateLogSource(name);
        }

        public void Log(LogLevel level, string message) {
            _logger.Log(level, message);
        }

        public void LogDebug(string message) {
            _logger.LogDebug(message);
        }

        public void LogInfo(string message) {
            _logger.LogInfo(message);
        }

        public void LogWarning(string message) {
            _logger.LogWarning(message);
        }

        public void LogError(string message) {
            _logger.LogError(message);
        }
    }
}
