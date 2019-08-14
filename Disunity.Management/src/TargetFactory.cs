using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Newtonsoft.Json;


namespace Disunity.Management {

    public class TargetFactory {

        private readonly IFileSystem _fileSystem;
        
        public TargetFactory(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }
        
        public TargetFactory(): this(new FileSystem()) {}

        public Target FromFile(string path) {
            if (!_fileSystem.File.Exists(path)) return null;

            using (var file = _fileSystem.File.OpenText(path)) {
                var serializer = new JsonSerializer();
                return serializer.Deserialize(file, typeof(Target)) as Target;
            }
        }

        public Target FromJson(string json) {
            if (string.IsNullOrEmpty(json)) return null;

            var target = JsonConvert.DeserializeObject<Target>(json);
            return target;
        }

        public IEnumerable<Target> LoadAllFromPath(string path) {
            var targets = from directory in _fileSystem.Directory.EnumerateDirectories(path) 
                           select Path.Combine(directory, "target-info.json") into jsonPath
                           where _fileSystem.File.Exists(jsonPath)
                           select FromFile(jsonPath);
            return targets.ToList();
        }

    }

}