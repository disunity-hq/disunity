using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Disunity.Core.Archives {

    public class FileArchive : BaseArchive {

        public string Root { get; }

        public FileArchive(Func<string, Manifest> manifestFactory, string root) : base(manifestFactory) {
            Root = root;
        }

        public override bool HasEntry(string filename) {
            return File.Exists(Path.Combine(Root, filename));
        }

        public override Stream GetEntry(string filename) {
            if (!HasEntry(filename)) {
                return null;
            }

            var path = Path.Combine(Root, filename);
            return File.Open(path, FileMode.Open);
        }

        private IEnumerable<string> PathsRelativeTo(string subDir) {
            var path = Path.Combine(Root, subDir);
            var pathLength = path.Length;

            return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                            .Select(p => p.Remove(0, pathLength));
        }

        public override IEnumerable<string> ArtifactPaths => PathsRelativeTo("artifacts");
        public override IEnumerable<string> PreloadAssemblyPaths => PathsRelativeTo("preload");
        public override IEnumerable<string> RuntimeAssemblyPaths => PathsRelativeTo("runtime");
        public override IEnumerable<string> PrefabBundlePaths => PathsRelativeTo("prefabs");
        public override IEnumerable<string> SceneBundlePaths => PathsRelativeTo("scenes");

    }

}