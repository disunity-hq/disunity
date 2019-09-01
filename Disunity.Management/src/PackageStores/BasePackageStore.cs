using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Management.Services;
using Disunity.Management.Util;

using Microsoft.Extensions.Options;


namespace Disunity.Management.PackageStores {

    public abstract class BasePackageStore : IPackageStore {

        protected readonly IFileSystem FileSystem;
        protected readonly ISymbolicLink SymbolicLink;
        protected readonly IZipUtil ZipUtil;
        protected readonly PackageStoreOptions Options;

        public BasePackageStore(IOptionsMonitor<PackageStoreOptions> optionsAccessor, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil) {
            Options = optionsAccessor.CurrentValue;
            FileSystem = fileSystem;
            SymbolicLink = symbolicLink;
            ZipUtil = zipUtil;

            if (!FileSystem.Directory.Exists(Options.Path)) {
                FileSystem.Directory.CreateDirectory(Options.Path);
            }
        }

        public string GetPackagePath(string fullPackageName) {
            var path = FileSystem.Path.Combine(Options.Path, fullPackageName);
            return FileSystem.Directory.Exists(path) ? path : null;
        }

        public async Task<bool> CreatePackageReference(string fullPackageName, string path) {
            var packagePath = GetPackagePath(fullPackageName);
            if (packagePath == null) return false;

            await Task.Run(() => SymbolicLink.CreateDirectoryLink(path, packagePath));

            return true;
        }

        public async Task<string> DownloadPackage(string fullPackageName, bool force = false, CancellationToken cancellationToken = default) {
            var downloadUrl = await GetDownloadUrl(fullPackageName, cancellationToken);
            if (downloadUrl == null || cancellationToken.IsCancellationRequested) return null;

            var extractPath = FileSystem.Path.Combine(Options.Path, fullPackageName);

            if (FileSystem.Directory.Exists(extractPath)) return extractPath;

            return await ZipUtil.ExtractOnlineZip(downloadUrl, extractPath);
        }

        public void GarbageCollect() {
            throw new System.NotImplementedException();
        }

        public Task Clear() {
            return Task.Run(() => FileSystem.Directory.Delete(Options.Path, true));
        }

        public abstract Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default);

    }

}