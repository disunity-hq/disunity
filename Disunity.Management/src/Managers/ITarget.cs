using System.Collections.Generic;

using Disunity.Management.Models;

namespace Disunity.Management.Managers {

    /// <summary>
    /// Contains management features for targets
    /// </summary>
    public interface ITarget {
        
        /// <summary>
        /// The meta data for this managed target
        /// </summary>
        TargetMeta TargetMeta { get; }
        
        /// <summary>
        /// The currently active profile.
        /// </summary>
        /// <remarks>
        /// Setting this property will cause the active profile to be altered in the internal db,
        /// however it is up to the caller to synchronize the filesystem after the change
        /// </remarks>
        IProfile ActiveProfile { get; set; }
        
        /// <summary>
        /// A set of all profiles associated with this target.
        /// </summary>
        /// <remarks>
        /// Modifying this set will automatically adjust the internal db, however it is up to the caller
        /// to synchronize the filesystem after changes are made
        /// </remarks>
        ISet<IProfile> Profiles { get; }

        /// <summary>
        /// Removes the target from the internal db
        /// </summary>
        /// <remarks>
        /// Be aware that it is up to the caller to synchronize the filesystem after calling this method 
        /// </remarks>
        void Delete();
    }

}