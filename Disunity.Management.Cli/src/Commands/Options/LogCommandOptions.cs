using System.Collections.Generic;

using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    [Verb("log", HelpText = "Echo input to console")]
    [ChildVerbs(typeof(LogReplyCommandOptions), typeof(LogCountCommandOptions))]
    public class LogCommandOptions : CommandOptionsBase {

        [Value(0, HelpText = "Input Text", Required = true)]
        public IEnumerable<string> Input { get; set; }

    }

    [Verb("reply", HelpText = "Reply with the input")]
    public class LogReplyCommandOptions : LogCommandOptions { }
    
    [Verb("count", HelpText = "Counts the input length")]
    public class LogCountCommandOptions : LogCommandOptions { }

}