using System;
using System.IO;
using System.IO.Abstractions;

using Newtonsoft.Json;


namespace Disunity.Management {

    public class ProfileFactory : IProfileFactory {

        private readonly IFileSystem _fileSystem;

        public ProfileFactory(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public ProfileFactory() : this(new FileSystem()) { }

        public Profile Load(string path) {
            if (path == null) return null;

            var metaFilePath = GetProfileMetaFilePath(path);

            if (!_fileSystem.File.Exists(metaFilePath)) return null;

            using (var file = _fileSystem.File.OpenText(metaFilePath)) {
                var text = file.ReadToEnd();
                file.BaseStream.Seek(0, SeekOrigin.Begin);
                var serializer = new JsonSerializer();
                var profile = serializer.Deserialize(file, typeof(Profile)) as Profile;

                if (profile != null) {
                    profile.Path = path;
                }

                return profile;

            }
        }

        public Profile Create(string path, string displayName) {
            var profileDirName = Guid.NewGuid().ToString().Substring(0, 8);

            var profilePath = _fileSystem.Path.Combine(path, profileDirName);

            var profile = new Profile {
                Path = profilePath,
                DisplayName = displayName
            };
            
            CreateMetaFile(profile);

            return profile;
        }

        public Profile CreateExactPath(string path, string displayName) {
            var profile = new Profile {
                Path = path,
                DisplayName = displayName
            };
            
            CreateMetaFile(profile);

            return profile;
        }

        private void CreateMetaFile(Profile profile) {
            _fileSystem.Directory.CreateDirectory(profile.Path);

            var metaFilePath = GetProfileMetaFilePath(profile.Path); 
            var json = JsonConvert.SerializeObject(profile, Formatting.Indented);

            using (var file = _fileSystem.File.CreateText(metaFilePath)) {
                file.Write(json);
                file.Flush();
            }
        }

        private string GetProfileMetaFilePath(string profilePath) {
            return _fileSystem.Path.Combine(profilePath, "meta.json");
        }

    }

}