using System.Threading.Tasks;

using CommandLine;

using Disunity.Core;

using Ninject;


namespace Disunity.Management.Cli.Commands {

    public interface ICommandBase {

        Task Execute();

    }

    /// <summary>
    /// Base class that serves as an interface for execution 
    /// and provides common properties and components for commands.
    /// </summary>
    /// <remarks>
    /// General pattern is that commands that have child verbs should be left abstract
    /// and provide utilities, while leaf commands that do the actual work should be concrete.
    /// </remarks>
    public abstract class CommandBase {

        [Option('v', "verbose", HelpText = "Print details during execution")]
        public bool Verbose { get; set; }

        [Inject] public ILogger Logger { get; set; }

        public abstract Task Execute();

        protected void WriteLog(string message) {
            Logger.LogInfo(message);
        }

        protected void WriteError(string message) {
            Logger.LogError(message);
        }

        protected void WriteWarning(string message) {
            Logger.LogWarning(message);
        }

        protected void WriteVerbose(string message) {
            if (Verbose) {
                Logger.LogDebug(message);
            }
        }

    }

}