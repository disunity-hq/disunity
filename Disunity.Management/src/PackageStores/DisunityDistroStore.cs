using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public class DisunityDistroStore : BasePackageStore {

        private readonly IDisunityClient _disunityClient;
        private readonly HttpClient _httpClient;

        public DisunityDistroStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil, IDisunityClient disunityClient) : base(rootPath, fileSystem, symbolicLink, zipUtil) {
            _disunityClient = disunityClient;
            _httpClient = disunityClient.HttpClient;
        }
        
        public override async Task<string> GetDownloadUrl(string fullPackageName) {
            var versionNumber = fullPackageName.Substring("disunity_".Length);
            var allVersions = await _disunityClient.GetDisunityVersionsAsync();
            var foundVersion = allVersions.SingleOrDefault(v => v.VersionNumber == versionNumber);

            return foundVersion?.Url;
        }

    }

}