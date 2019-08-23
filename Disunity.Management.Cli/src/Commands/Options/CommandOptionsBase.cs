using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    public class CommandOptionsBase {

        [Option('v', "verbose", HelpText = "Print details during execution")]
        public bool Verbose { get; set; }
        
        

    }

}