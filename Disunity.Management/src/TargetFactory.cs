using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Disunity.Management.Util;

using Newtonsoft.Json;


namespace Disunity.Management {

    public class TargetFactory : ITargetFactory {

        private readonly IFileSystem _fileSystem;
        private readonly IProfileFactory _profileFactory;
        private readonly ISymbolicLink _symbolicLink;

        public string ManagedRoot { get; set; }

        public TargetFactory(IFileSystem fileSystem, IProfileFactory profileFactory, ISymbolicLink symbolicLink) {
            _fileSystem = fileSystem;
            _profileFactory = profileFactory;
            _symbolicLink = symbolicLink;
        }

        public TargetFactory(ISymbolicLink symbolicLink): this(new FileSystem(), new ProfileFactory(), symbolicLink) {}

        public Target FromFile(string path) {
            if (!_fileSystem.File.Exists(path)) return null;

            using (var file = _fileSystem.File.OpenText(path)) {
                var serializer = new JsonSerializer();
                var target = serializer.Deserialize(file, typeof(Target)) as Target;

                return target;
            }
        }

        public List<Target> LoadAllFromPath(string path) {
            var targets = from directory in _fileSystem.Directory.EnumerateDirectories(path)
                           select Path.Combine(directory, "target-info.json") into jsonPath
                           where _fileSystem.File.Exists(jsonPath)
                           select FromFile(jsonPath);
            return targets.ToList();
        }

        public Target CreateManagedTarget(string executablePath, string displayName, string slug) {
            var target = new Target {
                DisplayName = displayName,
                ExecutableName = Path.GetFileName(executablePath),
                TargetPath = Path.GetDirectoryName(executablePath),
                Slug = slug
            };

            var targetDir = Crypto.CalculateManagedPath(target);
            target.ManagedPath = Path.Combine(ManagedRoot, targetDir);

            var defaultProfilePath = _fileSystem.Path.Combine(target.ManagedPath, "profiles", "default");
            var defaultProfile = _profileFactory.CreateExactPath(defaultProfilePath,"Default");
            
            target.SetActiveProfile(_fileSystem,_symbolicLink, defaultProfile);

            return target;
        }


    }

}
