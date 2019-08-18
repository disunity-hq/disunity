using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

using Disunity.Core.Archives;
using Disunity.Management.Util;

using Newtonsoft.Json;


namespace Disunity.Management.Models {

    [JsonObject(MemberSerialization.OptIn)]
    public partial class Target {

        [JsonProperty] public string TargetPath { get; set; }
        [JsonProperty] public string ExecutableName { get; set; }

        [JsonProperty] public string DisplayName { get; set; }

        [JsonProperty] public string Slug { get; set; }
        
        [JsonProperty] public Dictionary<string,VersionRange> ModVersionMap { get; }

        public string ManagedPath { get; set; }

        public string ExecutablePath => Path.Combine(TargetPath, ExecutableName);

        public string MetaFilePath => Path.Combine(TargetPath, "target-info.json");

    }

    public partial class Target {

        public void SetActiveProfile(IFileSystem fileSystem, ISymbolicLink symbolicLink, Profile profile) {
            var activeProfilePath = fileSystem.Path.Combine(ManagedPath, "profiles", "active");

            // remove old link if it exists
            if (fileSystem.File.Exists(activeProfilePath)) {
                fileSystem.File.Delete(activeProfilePath);
            }

            symbolicLink.CreateDirectoryLink(activeProfilePath, profile.Path);
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

            return Equals((Target) obj);
        }

        protected bool Equals(Target target) {
            return string.Equals(TargetPath, target.TargetPath) && string.Equals(ExecutableName, target.ExecutableName) && string.Equals(DisplayName, target.DisplayName) && string.Equals(Slug, target.Slug) && string.Equals(ManagedPath, target.ManagedPath);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (TargetPath != null ? TargetPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExecutableName != null ? ExecutableName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Slug != null ? Slug.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ManagedPath != null ? ManagedPath.GetHashCode() : 0);
                return hashCode;
            }
        }

        public void Delete(IFileSystem fileSystem) {
            fileSystem.Directory.Delete(ManagedPath, true);
        }

    }

}