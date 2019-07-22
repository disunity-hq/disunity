using BepInEx.Logging;

using Disunity.Core;

using LogLevel = Disunity.Core.LogLevel;


namespace Disunity.Preloader {

    public class PreloadLogger : ILogger {

        protected ManualLogSource _logger;

        public PreloadLogger(string name) {
            _logger = Logger.CreateLogSource(name);
        }

        public void Log(LogLevel level, string message) {
            _logger.Log((BepInEx.Logging.LogLevel) level, message);
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