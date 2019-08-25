using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Management.Models;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Newtonsoft.Json;


namespace Disunity.Management.Factories {

    [AsSingleton(typeof(ITargetFactory))]
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

        public async Task<Target> FromFile(string path) {
            if (!_fileSystem.File.Exists(path)) return null;

            using (var file = _fileSystem.File.OpenText(path)) {
                var serializer = new JsonSerializer();
                var target = await Task.Run(() => serializer.Deserialize(file, typeof(Target)) as Target);

                return target;
            }
        }

        public async Task<List<Target>> LoadAllFromPath(string path) {
            var targets = from directory in _fileSystem.Directory.EnumerateDirectories(path)
                           select Path.Combine(directory, "target-info.json") into jsonPath
                           where _fileSystem.File.Exists(jsonPath)
                           select FromFile(jsonPath);
            
            
            
            return (await Task.WhenAll(targets)).ToList();
        }

        public async Task<Target> CreateManagedTarget(string executablePath, string displayName, string slug, int? hashLength=null) {
            var target = new Target {
                DisplayName = displayName,
                ExecutableName = Path.GetFileName(executablePath),
                TargetPath = Path.GetDirectoryName(executablePath),
                Slug = slug
            };

            var targetDir = Crypto.CalculateManagedPath(target, hashLength);
            target.ManagedPath = Path.Combine(ManagedRoot, targetDir);

            var defaultProfilePath = _fileSystem.Path.Combine(target.ManagedPath, "profiles", "default");
            var defaultProfile = await _profileFactory.CreateExactPath(defaultProfilePath,"Default");
            
            target.SetActiveProfile(_fileSystem,_symbolicLink, defaultProfile);

            return target;
        }


    }

}
