using System.IO.Abstractions;

using Newtonsoft.Json;


namespace Disunity.Management {

    public class Profile {

        public string DisplayName { get; set; }

        [JsonIgnore]
        public string Path { get; set; }

        protected bool Equals(Profile other) {
            return string.Equals(DisplayName, other.DisplayName) && string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return Equals((Profile) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((DisplayName != null ? DisplayName.GetHashCode() : 0) * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            }
        }

        public void Delete(IFileSystem fileSystem) {
            fileSystem.Directory.Delete(Path, true);
        }

    }

}