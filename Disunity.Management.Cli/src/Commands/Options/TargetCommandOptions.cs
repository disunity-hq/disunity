using System.Collections.Generic;

using CommandLine;
using CommandLine.Text;

using SemVersion;


namespace Disunity.Management.Cli.Commands.Options {

    [Verb("target", HelpText = "Manage modded targets being tracked")]
    [ChildVerbs(typeof(TargetInitOptions), typeof(TargetDropCommandOptions), typeof(TargetListCommandOptions))]
    public abstract class TargetCommandOptions : CommandOptionsBase { }

    [Verb("init", HelpText = "Begin tracking a target and configure it with disunity")]
    public class TargetInitOptions : TargetCommandOptions {

        [Value(0, MetaName = "executable-path", HelpText = "Absolute or relative path to target executable", Required = true)]
        public string ExecutablePath { get; set; }

        [Option("distro", HelpText = "Which version of the disunity distro to install", Default = "latest")]
        public string DistroVersion { get; set; }

    }

    [Verb("drop", HelpText = "Stop tracking target and cleanup disunity files")]
    public class TargetDropCommandOptions : TargetCommandOptions, ITargetSpecificCommandOptions {

        public string Target { get; set; }

    }

    [Verb("ls", HelpText = "List currently tracked mods")]
    public class TargetListCommandOptions : TargetCommandOptions { }

}