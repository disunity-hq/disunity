using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

using Disunity.Core.Archives;
using Disunity.Management.Models;
using Disunity.Management.Util;


namespace Disunity.Management {

    public interface ITargetManager {
        
        /// <summary>
        /// The <see cref="Target"/> being managed
        /// </summary>
        Target Target { get; }

        /// <summary>
        /// Add a <see cref="Mod"/> to this <see cref="Target"/>'s mod version registry
        /// </summary>
        /// <remarks>
        /// If the mod already exists in the registry, version will be updated
        /// </remarks>
        /// <param name="mod">The <see cref="Mod"/> to add</param>
        /// <param name="versionRange">The version requirements </param>
        /// <returns></returns>
        Task AddModToRegistry(Mod mod, VersionRange versionRange);

        /// <summary>
        /// Removes a <see cref="Mod"/> from the registry 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        Task RemoveModFromRegistry(Mod mod);

        /// <summary>
        /// Loads possible changes from disk to <see cref="Target"/>
        /// </summary>
        /// <returns></returns>
        Task SynchronizeMetaData();
        
        /// <summary>
        /// Installs mods into the active target profile and uninstalls mods not listed in the mod list
        /// </summary>
        /// <returns></returns>
        Task SynchronizeReferences(IEnumerable<ModVersion> mods);

        /// <summary>
        /// Changes the active profile on the managed target to the specified one
        /// </summary>
        /// <param name="profile">The new <see cref="Profile"/> to set as active</param>
        /// <returns></returns>
        Task SetActiveProfile(Profile profile);

        /// <summary>
        /// Stop managing mods for this target and clean-up all necessary files
        /// </summary>
        /// <remarks>
        /// Should remove the doorstop hook from target install location and clean-up the managed target folder
        /// </remarks>
        /// <returns></returns>
        Task StopTrackingTarget();

    }

}