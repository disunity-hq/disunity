using System.Threading.Tasks;


namespace Disunity.Management {

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
        /// <param name="fullPackageName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task CreatePackageReference(string fullPackageName, string path);

        /// <summary>
        /// Download the specified package into the package store.
        /// </summary>
        /// <remarks>
        /// Will skip download by default if the specified package has already been downloaded
        /// </remarks>
        /// <param name="fullPackageName">The full package name (ie owner_mod_version or disunity_version)</param>
        /// <param name="force">When true, will always download, even if the specified mod is already downloaded</param>
        /// <returns>The absolute path to the downloaded package</returns>
        Task<string> DownloadPackage(string fullPackageName, bool force=false);

        /// <summary>
        /// Wipes the store and removes all downloaded entities
        /// </summary>
        /// <returns></returns>
        Task Clear();

    }

}