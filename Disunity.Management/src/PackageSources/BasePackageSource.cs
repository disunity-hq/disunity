using System.IO;
using System.Threading.Tasks;

using Disunity.Management.Models;


namespace Disunity.Management.PackageSources {

    public abstract class BasePackageSource : IPackageSource {

        public string SourceUri { get; set; }
        public abstract Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier);
        public abstract Task<bool> CanHandlePackage(PackageIdentifier packageIdentifier);

    }

}