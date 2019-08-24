using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public class ModPackageStore : BasePackageStore {

        public const string DownloadUrlBase = "https://disunity.io/u";

        public ModPackageStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil) : base(rootPath, fileSystem, symbolicLink, zipUtil) { }

        public override Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken) {
            return Task.FromResult($"{DownloadUrlBase}/{fullPackageName}/download");
        }

    }

}