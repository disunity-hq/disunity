using SemVersion;


namespace Disunity.Management.Models {

    /// <summary>
    /// Represents a unique identifier for generic packages
    /// </summary>
    public class PackageIdentifier {

        /// <summary>
        /// Id string for this package. Should be the minimal string necessary to uniquely identify this package excluding the version
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The version 
        /// </summary>
        public SemanticVersion Version { get; }

        public PackageIdentifier(string id) {
            Id = id;
            Version = null;
        }
        
        public PackageIdentifier(string id, string version) : this(id, SemanticVersion.Parse(version)) { }

        public PackageIdentifier(string id, SemanticVersion version) {
            Id = id;
            Version = version;
        }

        /// <summary>
        /// Check whether the stored <see cref="Id"/> is validate. Default implementation always returns true
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate() => true;

        public override string ToString() {
            return $"{Id}_{Version}";
        }

    }

}