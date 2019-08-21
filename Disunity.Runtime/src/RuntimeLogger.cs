using BepInEx.Logging;

using Disunity.Core;

using LogLevel = Disunity.Core.LogLevel;


namespace Disunity.Runtime {

    public class RuntimeLogger : LoggerBase {

        protected ManualLogSource _logger;

        public RuntimeLogger(string name) {
            _logger = Logger.CreateLogSource(name);
        }

        public override void Log(LogLevel level, string message) {
            _logger.Log((BepInEx.Logging.LogLevel) level, message);
        }

    }

}