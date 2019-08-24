using System.Threading;
using System.Threading.Tasks;


namespace Disunity.Management.PackageStores {

    /// <summary>
    /// A general interface for handling downloading and storing the various online archives (mods, disunity distro, etc)
    /// </summary>
    public interface IPackageStore {

        /// <summary>
        /// Get the absolute path for a given package
        /// </summary>
        /// <param name="fullPackageName">The full package name (ie owner_mod_version or disunity_version)</param>
        /// <returns>The absolute path to the package specified, or null if not found</returns>
        string GetPackagePath(string fullPackageName);

        /// <summary>
        /// Create a symlink at <see cref="path"/> pointing to the specified package 
        /// </summary>
        /// <param name="fullPackageName">The package to link to</param>
        /// <param name="path">The path at which to create the symlink</param>
        /// <returns></returns>
        Task<bool> CreatePackageReference(string fullPackageName, string path);

        /// <summary>
        /// Download the specified package into the package store.
        /// </summary>
        /// <remarks>
        /// Will skip download by default if the specified package has already been downloaded
        /// </remarks>
        /// <param name="fullPackageName">The full package name (ie owner_mod_version or disunity_version)</param>
        /// <param name="force">When true, will always download, even if the specified mod is already downloaded</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The absolute path to the downloaded package</returns>
        Task<string> DownloadPackage(string fullPackageName, bool force = false, CancellationToken cancellationToken=default);

        /// <summary>
        /// Retrieve the download url for a given package 
        /// </summary>
        /// <param name="fullPackageName">The full package name (ie owner_mod_version or disunity_version)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The url the package can be downloaded from or null</returns>
        Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken=default);

        /// <summary>
        /// Wipes the store and removes all downloaded entities
        /// </summary>
        /// <returns></returns>
        Task Clear();

    }

}