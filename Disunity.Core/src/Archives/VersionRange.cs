using SemVersion;


namespace Disunity.Core.Archives {

    public class VersionRange {

        public static VersionRange Unbound = new VersionRange();

        public string MaxVersion = null;

        public string MinVersion = null;

        public VersionRange(string min = null, string max = null) {
            MinVersion = min;
            MaxVersion = max;
        }

        public override string ToString() {
            return $"{MinVersion} - {MaxVersion}";
        }

        public override bool Equals(object obj) {
            return obj is VersionRange range &&
                   MinVersion == range.MinVersion &&
                   MaxVersion == range.MaxVersion;
        }

        public string Validate() {

            SemanticVersion min = null;
            SemanticVersion max = null;
            
            if (MinVersion != null && SemanticVersion.TryParse(MinVersion, out min)) {
                return "MinVersion must be a valid Semantic Version is specified";
            }
            
            if (MaxVersion != null && SemanticVersion.TryParse(MaxVersion, out max)) {
                return "MaxVersion must be a valid Semantic Version is specified";
            }

            if (min == null || max == null || min < max) {
                return null;
            }

            return "MinVersion is higher than or equal to MaxVersion";
        }

    }

}