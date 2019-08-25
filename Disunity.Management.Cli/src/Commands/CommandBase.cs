using System.Threading;
using System.Threading.Tasks;

using Disunity.Core;
using Disunity.Management.Cli.Commands.Options;


namespace Disunity.Management.Cli.Commands {

    public interface ICommandBase<T> where T : CommandOptionsBase {

        Task Execute(T options, CancellationToken cancellationToken=default);

    }

    /// <summary>
    /// Base class that serves as an interface for execution 
    /// and provides common properties and components for commands.
    /// </summary>
    /// <remarks>
    /// General pattern is that commands that have child verbs should be left abstract
    /// and provide utilities, while leaf commands that do the actual work should be concrete.
    /// </remarks>
    public abstract class CommandBase<T> : ICommandBase<T> where T : CommandOptionsBase {

        private readonly ILogger _logger;

        protected T Options { get; set; }

        public CommandBase(ILogger logger) {
            _logger = logger;
        }

        public async Task Execute(T options, CancellationToken cancellationToken=default) {
            Options = options;
            await Execute(cancellationToken);
            Options = null;
        }

        protected abstract Task Execute(CancellationToken cancellationToken);

        protected void WriteLog(string message) {
            _logger.LogInfo(message);
        }

        protected void WriteError(string message) {
            _logger.LogError(message);
        }

        protected void WriteWarning(string message) {
            _logger.LogWarning(message);
        }

        protected void WriteVerbose(string message) {
            if (Options.Verbose) {
                _logger.LogDebug(message);
            }
        }

    }

}