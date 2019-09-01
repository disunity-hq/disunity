using System.Collections.Generic;

using Disunity.Management.Managers;
using Disunity.Management.PackageStores;


namespace Disunity.Management.Models {

    /// <summary>
    /// Represents a resolved 
    /// </summary>
    public interface IPackage {

        PackageIdentifier Id { get; set; }
        IReadOnlyCollection<ProfileMeta> References { get; }
        
//        IPackageStore Source { get; set; }

        void CreateReference(ProfileMeta profile);

        void RemoveReference(ProfileMeta profile);

    }

}