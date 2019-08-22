using System.Collections.Generic;
using System.Threading.Tasks;

using CommandLine;


namespace Disunity.Management.Cli.Commands {

    [Verb("log", HelpText = "Echo input to console")]
    [ChildVerbs(typeof(LogReplyCommand), typeof(LogCountCommand))]
    public abstract class LogCommand : CommandBase {

        [Value(0, HelpText = "Input Text", Required = true)]
        public IEnumerable<string> Input { get; set; }

    }

    [Verb("reply", HelpText = "Reply with the input")]

    public class LogReplyCommand : LogCommand {

        public override Task Execute() {
            WriteLog(string.Join(' ', Input));

            return Task.CompletedTask;
        }

    }

    [Verb("count", HelpText = "Counts the input length")]

    public class LogCountCommand : LogCommand {

        public override Task Execute() {
            WriteLog(string.Join(' ', Input).Length.ToString());

            return Task.CompletedTask;
        }

    }

}