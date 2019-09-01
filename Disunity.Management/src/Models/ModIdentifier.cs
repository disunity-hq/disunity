using System.Text.RegularExpressions;

using Disunity.Core.Archives;


namespace Disunity.Management.Models {

    public class ModIdentifier:PackageIdentifier {

        private static readonly string VersionPattern = Schema.VERSION_PATTERN.Substring(1, Schema.VERSION_PATTERN.Length - 2);
        private static readonly string SlugPattern = Schema.SLUG_PATTERN.Substring(1, Schema.SLUG_PATTERN.Length - 2);
        private static readonly string Pattern =$"({SlugPattern})_({SlugPattern})_({VersionPattern})";

        public string OwnerSlug => Regex.Match(Id, Pattern).Groups[1].Value;
        public string ModSlug => Regex.Match(Id, Pattern).Groups[2].Value;
        public string Version => Regex.Match(Id, Pattern).Groups[3].Value;

        public override bool Validate() {
            return Regex.IsMatch(Id, Pattern);
        }

    }

}