using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
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
    public class DisunityDistroStore : BasePackageStore {

        private readonly IDisunityClient _disunityClient;
        private readonly HttpClient _httpClient;

        public DisunityDistroStore(IOptionsMonitor<PackageStoreOptions> optionsAccessor, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil, IDisunityClient disunityClient) :
            base(optionsAccessor, fileSystem, symbolicLink, zipUtil) {
            _disunityClient = disunityClient;
            _httpClient = disunityClient.HttpClient;
        }

        public override async Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default) {
            var versionNumber = fullPackageName.Substring("disunity_".Length);
            var allVersions = await _disunityClient.GetDisunityVersionsAsync(cancellationToken);
            var foundVersion = allVersions.SingleOrDefault(v => v.VersionNumber == versionNumber);

            return foundVersion?.Url;
        }

    }

}