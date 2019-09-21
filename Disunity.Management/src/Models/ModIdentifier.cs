using System.Text.RegularExpressions;

using Disunity.Core.Archives;


namespace Disunity.Management.Models {

    public class ModIdentifier : PackageIdentifier {

        private static readonly string VersionPattern = Schema.VERSION_PATTERN.Substring(1, Schema.VERSION_PATTERN.Length - 2);
        private static readonly string SlugPattern = Schema.SLUG_PATTERN.Substring(1, Schema.SLUG_PATTERN.Length - 2);
        private static readonly string Pattern = $"({SlugPattern})_({SlugPattern})";

        /// <summary>
        /// The owner segment parsed out of the <see cref="PackageIdentifier.Id"/>
        /// </summary>
        public string OwnerSlug => Regex.Match(Id, Pattern).Groups[1].Value;

        /// <summary>
        /// The mod segment parsed out of the <see cref="PackageIdentifier.Id"/>
        /// </summary>
        public string ModSlug => Regex.Match(Id, Pattern).Groups[2].Value;

        public ModIdentifier(string owner, string mod) : base($"{owner}_{mod}") { }

        public ModIdentifier(string owner, string mod, string version) : base($"{owner}_{mod}", version) { }

        public override bool Validate() {
            return Regex.IsMatch(Id, Pattern);
        }

    }

}