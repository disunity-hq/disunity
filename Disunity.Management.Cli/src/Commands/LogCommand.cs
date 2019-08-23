using System.Threading.Tasks;

using Disunity.Core;
using Disunity.Management.Cli.Commands.Options;


namespace Disunity.Management.Cli.Commands {

    public class LogReplyCommand: CommandBase<LogReplyCommandOptions> {

        public LogReplyCommand(ILogger logger) : base(logger) { }

        protected override Task Execute() {
            WriteLog(string.Join(' ', Options.Input));

            return Task.CompletedTask;
        }

    }
    public class LogCountCommand: CommandBase<LogCountCommandOptions> {

        public LogCountCommand(ILogger logger) : base(logger) { }

        protected override Task Execute() {
            WriteLog(string.Join(' ', Options.Input).Length.ToString());

            return Task.CompletedTask;
        }

    }

}