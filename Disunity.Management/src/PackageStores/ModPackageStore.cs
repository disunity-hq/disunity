using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Client.v1;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace Disunity.Management.PackageStores {

    [AsSingleton]
    public class ModPackageStore : BasePackageStore {

        public const string DownloadUrlBase = "https://disunity.io/u";

        public ModPackageStore(IOptionsMonitor<PackageStoreOptions> optionsAccessor, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil) : base(optionsAccessor, fileSystem, symbolicLink, zipUtil) { }

        public override Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default) {
            return Task.FromResult($"{DownloadUrlBase}/{fullPackageName}/download");
        }

    }

}