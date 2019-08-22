using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public class DisunityDistroStore : BasePackageStore {

        private readonly IDisunityClient _disunityClient;
        private readonly HttpClient _httpClient;

        public DisunityDistroStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink, IDisunityClient disunityClient) : base(rootPath, fileSystem, symbolicLink) {
            _disunityClient = disunityClient;
            _httpClient = disunityClient.HttpClient;
        }

        public override Task<string> DownloadPackage(string fullPackageName, bool force = false) {
            throw new System.NotImplementedException();
        }

    }

}