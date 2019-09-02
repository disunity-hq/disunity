using System.IO;
using System.Threading.Tasks;

using Disunity.Management.Models;


namespace Disunity.Management.PackageSources {

    /// <summary>
    /// Base type for interacting with various package sources, be them disunity.io, a local directory,
    /// or a third-party service
    /// </summary>
    public interface IPackageSource {
        
        /// <summary>
        /// URI source to use to request packages from
        /// </summary>
        string SourceUri { get; set; }
        
        /// <summary>
        /// Get a stream from which the package specified by <see cref="packageIdentifier"/> can be imported into a store
        /// </summary>
        /// <param name="packageIdentifier">The unique identifier for the requested package</param>
        /// <returns></returns>
        Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier);

        /// <summary>
        /// Checks whether the given <see cref="PackageIdentifier"/> can be handled by this <see cref="IPackageSource"/>
        /// </summary>
        /// <remarks>
        /// IMPORTANT: This does NOT necessarily guarantee that a stream can be provided by <see cref="GetPackageImportStream"/>
        /// </remarks>
        /// <param name="packageIdentifier">The unique identifier for the requested package</param>
        /// <returns></returns>
        Task<bool> CanHandlePackage(PackageIdentifier packageIdentifier);

    }

}