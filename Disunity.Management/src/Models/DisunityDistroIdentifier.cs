using System.Text.RegularExpressions;

using Disunity.Core.Archives;


namespace Disunity.Management.Models {

    public class DisunityDistroIdentifier: PackageIdentifier {

        private static readonly string VersionPattern = Schema.VERSION_PATTERN.Substring(1, Schema.VERSION_PATTERN.Length - 2);
        private static readonly string Pattern = $"^disunity_({VersionPattern})$";

        /// <summary>
        /// The version segment parsed out of the <see cref="PackageIdentifier.Id"/>
        /// </summary>
        public string Version => Regex.Match(Id, Pattern).Groups[1].Value;

        public override bool Validate() {
            return Regex.IsMatch(Id, Pattern);
        }

    }

}