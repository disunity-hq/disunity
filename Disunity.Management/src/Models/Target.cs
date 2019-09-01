using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

using Disunity.Core.Archives;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Newtonsoft.Json;


namespace Disunity.Management.Models {

    public class Target {

        public Target(TargetMeta targetMeta) {
            TargetMeta = targetMeta;
        }

        public TargetMeta TargetMeta { get; }
        
        
        public void SetActiveProfile(IFileSystem fileSystem, ISymbolicLink symbolicLink, Profile profile) {
            var activeProfilePath = fileSystem.Path.Combine(TargetMeta.ManagedPath, "profiles", "active");

            // remove old link if it exists
            if (fileSystem.File.Exists(activeProfilePath)) {
                fileSystem.File.Delete(activeProfilePath);
            }

            symbolicLink.CreateDirectoryLink(activeProfilePath, profile.Path);
        }


        public void Delete(IFileSystem fileSystem) {
            fileSystem.Directory.Delete(TargetMeta.ManagedPath, true);
        }

    }

}