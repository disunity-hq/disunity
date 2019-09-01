using System.Collections;
using System.Collections.Generic;

using Disunity.Core.Archives;
using Disunity.Management.Models;


namespace Disunity.Management.Managers {

    /// <summary>
    /// Contains management features for Profiles
    /// </summary>
    public interface IProfile {

        /// <summary>
        /// The metadata for the managed profile.
        /// </summary>
        ProfileMeta ProfileMeta { get; }

        /// <summary>
        /// Absolute path to this profile's directory 
        /// </summary>
        string ProfilePath { get; }
        
        /// <summary>
        /// A dictionary containing the mods and version restrictions.
        /// </summary>
        IDictionary<ModIdentifier, VersionRange> ModVersions { get; }

        /// <summary>
        /// Delete this profile's backing files and cleanup meta data from the internal db
        /// </summary>
        void Delete();


    }

}