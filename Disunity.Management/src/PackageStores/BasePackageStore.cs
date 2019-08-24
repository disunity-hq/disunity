using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public abstract class BasePackageStore : IPackageStore {

        protected readonly string RootPath;
        protected readonly IFileSystem FileSystem;
        protected readonly ISymbolicLink SymbolicLink;
        protected readonly IZipUtil ZipUtil;

        public BasePackageStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil) {
            RootPath = rootPath;
            FileSystem = fileSystem;
            SymbolicLink = symbolicLink;
            ZipUtil = zipUtil;

            if (!FileSystem.Directory.Exists(RootPath)) {
                FileSystem.Directory.CreateDirectory(RootPath);
            }
        }

        public string GetPackagePath(string fullPackageName) {
            var path = FileSystem.Path.Combine(RootPath, fullPackageName);
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

            var extractPath = FileSystem.Path.Combine(RootPath, fullPackageName);

            if (FileSystem.Directory.Exists(extractPath)) return extractPath;

            return await ZipUtil.ExtractOnlineZip(downloadUrl, extractPath);
        }

        public Task Clear() {
            return Task.Run(() => FileSystem.Directory.Delete(RootPath, true));
        }

        public abstract Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default);

    }

}