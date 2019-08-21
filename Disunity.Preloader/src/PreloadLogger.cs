using BepInEx.Logging;

using Disunity.Core;

using LogLevel = Disunity.Core.LogLevel;


namespace Disunity.Preloader {

    public class PreloadLogger : LoggerBase {

        private readonly ManualLogSource _logger;

        public PreloadLogger(string name) {
            _logger = Logger.CreateLogSource(name);
        }

        public override void Log(LogLevel level, string message) {
            _logger.Log((BepInEx.Logging.LogLevel) level, message);
        }
        

    }

}