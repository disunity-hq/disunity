using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;


namespace Disunity.Management.PackageStores {

    public class ModPackageStore : BasePackageStore {

        public const string DownloadUrlBase = "https://disunity.io/u";

        public ModPackageStore(IConfiguration config, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil) : base(config["PackageStore:Disunity:Path"] ?? "~/.disunity/store/mods", fileSystem, symbolicLink, zipUtil) { }

        public override Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default) {
            return Task.FromResult($"{DownloadUrlBase}/{fullPackageName}/download");
        }

    }

}