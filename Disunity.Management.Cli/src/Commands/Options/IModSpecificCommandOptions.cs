using System.Linq;


using CommandLine;


namespace Disunity.Management.Cli.Commands.Options {

    public interface IModSpecificCommandOptions {

        [Value(1, Required = true, MetaName = "mod", HelpText = "The unique identifier for this mod")]
        string NameSegments { get; set; }

    }

    public static class ModSpecificCommandOptionExtensions {

        public static string GetAuthorSlug(this IModSpecificCommandOptions options) {
            return options.NameSegments.Split('/')[0];
        }

        public static string GetModSlug(this IModSpecificCommandOptions options) {
            return options.NameSegments.Split('/')[1];
        }

        public static bool ModNameIsValid(this IModSpecificCommandOptions options) {
            return options.NameSegments.Split('/').Count(s => !string.IsNullOrEmpty(s)) == 2;
        }

    }

}