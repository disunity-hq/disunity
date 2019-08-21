using System.IO.Abstractions;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public class ModPackageStore : BasePackageStore {

        private readonly IModListClient _modListClient;

        public ModPackageStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink, IModListClient modListClient) : base(rootPath, fileSystem, symbolicLink) {
            _modListClient = modListClient;
        }

        public override Task<string> DownloadPackage(string fullPackageName, bool force = false) {
            throw new System.NotImplementedException();
        }

    }

}