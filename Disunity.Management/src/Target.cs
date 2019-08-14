using System.IO;

using Newtonsoft.Json;


namespace Disunity.Management {

    public class Target {

        public string TargetPath { get; set; }
        public string ExecutableName { get; set; }

        public string DisplayName { get; set; }

        [JsonIgnore]
        public string ExecutablePath => Path.Combine(TargetPath, ExecutableName);

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (!(obj is Target other)) return false;
            
            return Equals(other);
        }

        protected bool Equals(Target other) {
            return string.Equals(TargetPath, other.TargetPath) && string.Equals(ExecutableName, other.ExecutableName) && string.Equals(DisplayName, other.DisplayName);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (TargetPath != null ? TargetPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExecutableName != null ? ExecutableName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                return hashCode;
            }
        }

    }

}