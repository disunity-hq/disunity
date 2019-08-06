using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Disunity.Core.Archives {

    public abstract class BaseArchive : IArchive {

        private readonly Func<string, Manifest> _manifestFactory;
        private Manifest _manifest;
        private string _readme;

        public BaseArchive(Func<string, Manifest> manifestFactory) {
            _manifestFactory = manifestFactory;
        }

        public abstract bool HasEntry(string filename);

        public abstract Stream GetEntry(string filename);

        public Manifest Manifest => _manifest ?? (_manifest = GetManifest());
        public string Readme => _readme ?? (_readme = GetReadmeContent());

        public bool HasReadme() {
            return HasEntry("README.md");
        }

        public bool HasManifest() {
            return HasEntry("manifest.json");
        }

        public bool HasArtifact(string filename) {
            return HasEntry(Path.Combine("artifacts", filename));
        }

        public bool HasPreloadAssembly(string filename) {
            return HasEntry(Path.Combine("preload", filename));
        }

        public bool HasRuntimeAssembly(string filename) {
            return HasEntry(Path.Combine("runtime", filename));
        }

        public bool HasPrefabBundle(string filename) {
            return HasEntry(Path.Combine("prefabs", filename));
        }

        public bool HasSceneBundle(string filename) {
            return HasEntry(Path.Combine("scenes", filename));
        }

        public Manifest GetManifest() {
            var entry = GetEntry("manifest.json");

            if (entry == null) {
                return null;
            }

            using (entry) {
                var reader = new StreamReader(entry);
                var text = reader.ReadToEnd();
                return _manifestFactory(text);
            }

        }

        public Stream GetReadme() {
            return GetEntry("README.md");
        }

        public Stream GetArtifact(string filename) {
            return GetEntry(Path.Combine("artifacts", filename));
        }

        public Stream GetPreloadAssembly(string filename) {
            return GetEntry(Path.Combine("preload", filename));
        }

        public Stream GetRuntimeAssembly(string filename) {
            return GetEntry(Path.Combine("runtime", filename));
        }

        public Stream GetPrefabBundle(string filename) {
            return GetEntry(Path.Combine("prefabs", filename));
        }

        public Stream GetSceneBundle(string filename) {
            return GetEntry(Path.Combine("scenes", filename));
        }

        public abstract IEnumerable<string> ArtifactPaths { get; }
        public abstract IEnumerable<string> PreloadAssemblyPaths { get; }
        public abstract IEnumerable<string> RuntimeAssemblyPaths { get; }
        public abstract IEnumerable<string> PrefabBundlePaths { get; }
        public abstract IEnumerable<string> SceneBundlePaths { get; }

        public string GetReadmeContent() {
            var stream = GetReadme();

            if (stream == null) {
                return "";
            }

            var encoding = new UTF8Encoding(false, true);

            using (var reader = new StreamReader(stream, encoding, true)) {
                var readme = reader.ReadToEnd();
                return readme.Length == 0 ? "" : readme;
            }
        }

    }

}