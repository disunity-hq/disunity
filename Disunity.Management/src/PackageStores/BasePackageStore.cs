using System.IO.Abstractions;
using System.Threading.Tasks;

using Disunity.Management.Util;


namespace Disunity.Management.PackageStores {

    public abstract class BasePackageStore : IPackageStore {

        protected readonly string RootPath;
        protected readonly IFileSystem FileSystem;
        protected readonly ISymbolicLink SymbolicLink;

        public BasePackageStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink) {
            RootPath = rootPath;
            FileSystem = fileSystem;
            SymbolicLink = symbolicLink;

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

        public Task Clear() {
            return Task.Run(() => FileSystem.Directory.Delete(RootPath, true));
        }

        public abstract Task<string> DownloadPackage(string fullPackageName, bool force = false);

    }

}