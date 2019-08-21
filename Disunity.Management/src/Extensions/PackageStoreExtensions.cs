using Disunity.Management.PackageStores;


namespace Disunity.Management.Extensions {

    public static class PackageStoreExtensions {

        /// <summary>
        /// A helper method for <see cref="IPackageStore.GetPackagePath"/> to provide the package name and version number separately
        /// </summary>
        /// <param name="store"></param>
        /// <param name="packageName">The package name (either owner_mod or disunity)</param>
        /// <param name="versionNumber">The version number to use</param>
        /// <returns></returns>
        public static string GetPackagePath(this IPackageStore store, string packageName, string versionNumber) {
            return store.GetPackagePath($"{packageName}_{versionNumber}");
        }

        /// <summary>
        /// A helper method for <see cref="IPackageStore.GetPackagePath"/> to provide the owner, mod and version number separately 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="owner"></param>
        /// <param name="modName"></param>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        public static string GetPackagePath(this IPackageStore store, string owner, string modName, string versionNumber) {
            return store.GetPackagePath($"{owner}_{modName}", versionNumber);
        }
    }

}