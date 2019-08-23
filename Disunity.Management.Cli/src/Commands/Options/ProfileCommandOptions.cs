using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    public interface IProfileSpecificCommandOptions {

        [Value(1, MetaName = "profile", Required = true, HelpText = "The name of the profile")]
        string Profile { get; set; }

    }

    [Verb("profile", HelpText = "Manage and switch between profiles")]
    [ChildVerbs(typeof(ProfileNewCommandOptions), typeof(ProfileActivateCommandOptions), typeof(ProfileListCommandOptions), typeof(ProfileDeleteCommandOptions))]
    public abstract class ProfileCommandOptions : CommandOptionsBase, ITargetSpecificCommandOptions {

        public string Target { get; set; }

    }

    [Verb("new", HelpText = "Create a new ")]
    public class ProfileNewCommandOptions : ProfileCommandOptions, IProfileSpecificCommandOptions {

        public string Profile { get; set; }

        [Option("copy-from", HelpText = "Copy an existing profile when creating this one")]
        public string CopyFrom { get; set; }

        [Option(HelpText = "Activate the profile after creating it")]
        public bool Activate { get; set; }

    }

    [Verb("activate", HelpText = "Activate a profile")]
    public class ProfileActivateCommandOptions : ProfileCommandOptions, IProfileSpecificCommandOptions {

        public string Profile { get; set; }

    }

    [Verb("delete", HelpText = "Deletes an existing profile")]
    public class ProfileDeleteCommandOptions : ProfileCommandOptions, IProfileSpecificCommandOptions {

        public string Profile { get; set; }

    }

    [Verb("ls", HelpText = "List all profiles for a target")]
    public class ProfileListCommandOptions : ProfileCommandOptions { }

}