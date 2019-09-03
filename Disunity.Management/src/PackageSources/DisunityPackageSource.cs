using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.Models;
using Disunity.Management.PackageStores;


namespace Disunity.Management.PackageSources {

    [PackageSource("disunity://", typeof(DisunityDistroStore), typeof(ModPackageStore))]
    public class DisunityPackageSource : BasePackageSource {

        private readonly IApiClient _client;

        public DisunityPackageSource(IApiClient client) {
            _client = client;

        }

        public override Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier) {
            switch (packageIdentifier) {
                case DisunityDistroIdentifier disunityDistroIdentifier:
                    return GetDisunityDistroImportStream(disunityDistroIdentifier);

                case ModIdentifier modIdentifier:
                    return GetModImportStream(modIdentifier);

                default:
                    throw new ArgumentOutOfRangeException(nameof(packageIdentifier), "Only Disunity distros and mods can be handled by this source");

            }
        }

        public override Task<bool> CanHandlePackage(PackageIdentifier packageIdentifier) {
            return Task.FromResult(true);
        }

        private async Task<Stream> GetDisunityDistroImportStream(DisunityDistroIdentifier packageId) {
            var versions = await _client.DisunityClient.GetDisunityVersionsAsync();
            var foundVersion = versions.SingleOrDefault(v => v.VersionNumber == packageId.Version);
            if (foundVersion == null) return null;
            return await _client.HttpClient.GetStreamAsync(foundVersion.Url);
        }

        private async Task<Stream> GetModImportStream(ModIdentifier packageId) {
            var allMods = await _client.ModListClient.GetModsAsync(null, null);
            var foundMod = allMods.SingleOrDefault(m => m.Owner.Slug == packageId.OwnerSlug && m.Slug == packageId.ModSlug);
            var foundVersion = foundMod?.Versions.SingleOrDefault(v => v.VersionNumber == packageId.Version);
            if (foundVersion == null) return null;
            return await _client.HttpClient.GetStreamAsync(foundVersion.FileUrl);
        }

    }

}