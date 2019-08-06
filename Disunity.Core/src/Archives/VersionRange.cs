using SemVersion;


namespace Disunity.Core.Archives {

    public class VersionRange {

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
            if (MinVersion == null || MaxVersion == null) {
                return null;
            }

            SemanticVersion min = MinVersion;
            SemanticVersion max = MaxVersion;

            if (min < max) {
                return null;
            }

            return "MinVersion is higher than or equal to MaxVersion";
        }

    }

}