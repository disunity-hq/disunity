using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    [Verb("mod", HelpText = "Manage mods installed in a given target")]
    [ChildVerbs(typeof(ModAddCommandOptions), typeof(ModRemoveCommandOptions), typeof(ModUpdateCommandOptions), typeof(ModListCommandOptions))]
    public abstract class ModCommandOptions : CommandOptionsBase, ITargetSpecificCommandOptions {

        public string Target { get; set; }

        [Option('p', "profile", HelpText = "The name of the profile to operate on", Default = "active")]
        public string Profile { get; set; }

    }

    [Verb("update", HelpText = "Update installed mods to latest version")]
    public class ModUpdateCommandOptions : ModCommandOptions { }

    [Verb("ls", HelpText = "List mods installed for the given profile")]
    public class ModListCommandOptions : ModCommandOptions { }

    [Verb("remove", HelpText = "Remove a mod from the given profile")]
    public class ModRemoveCommandOptions : ModCommandOptions, IModSpecificCommandOptions {

        public string NameSegments { get; set; }

    }

    [Verb("add", HelpText = "Add a mod to a given profile, downloading if necessary")]
    public class ModAddCommandOptions : ModCommandOptions, IModSpecificCommandOptions {

        public string NameSegments { get; set; }

        [Option("minVersion", HelpText = "The minimum version required to be installed")]
        public string MinVersion { get; set; }

        [Option("maxVersion", HelpText = "The maximum version required to be installed")]
        public string MaxVersion { get; set; }

    }

}