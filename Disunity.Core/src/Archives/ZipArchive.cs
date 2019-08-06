using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;


namespace Disunity.Core.Archives {

    public class ZipArchive : BaseArchive {

        public Stream Stream { get; }
        private readonly System.IO.Compression.ZipArchive _zip;

        private Manifest _manifest;
        private string _readme;

        public ZipArchive(Func<string, Manifest> manifestFactory, Stream stream) : base(manifestFactory) {
            Stream = stream;
            _zip = new System.IO.Compression.ZipArchive(stream, ZipArchiveMode.Read);
        }

        public override bool HasEntry(string filename) {
            return _zip.GetEntry(filename) != null;
        }
        
        public override Stream GetEntry(string filename) {
            var entry = _zip.GetEntry(filename);
            return entry?.Open();
        }

        public override IEnumerable<string> ArtifactPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("artifacts", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> PreloadAssemblyPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("preload", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> RuntimeAssemblyPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("runtime", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> PrefabBundlePaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("prefabs", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> SceneBundlePaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("scenes", StringComparison.Ordinal))
                    .Select(e => e.FullName);

    }

}