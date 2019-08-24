using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    public interface ITargetSpecificCommandOptions {

        [Value(0, Required = true, HelpText = "Path to target executable or the target's unique ID", MetaName = "target")]
        string Target { get; set; }

    }

}