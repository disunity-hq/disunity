using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Client.v1.Models;
using Disunity.Management.Models;


namespace Disunity.Management {

    public interface ITargetManagement {
        
        /// <summary>
        /// A list of all managed targets
        /// </summary>
        IReadOnlyList<Target> Targets { get; }

        /// <summary>
        /// Find and parse all managed target directories within <see cref="path"/>
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of all the found <see cref="Target"/>s.</returns>
        Task<List<Target>> LoadTargets();

        
        /// <summary>
        /// Configure a new target with the given disunity version or latest available version
        /// </summary>
        /// <param name="executablePath">The path to the target executable</param>
        /// <param name="disunityVersion">The desired disunity version to install in the default profile. Null for latest compatible version</param>
        /// <returns>The newly created <see cref="Target"/> or null</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Target> InitTarget(string executablePath, string disunityVersion=null);

    }

}