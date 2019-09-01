using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Disunity.Management.Managers {

    public interface ITargetManager {
        
        /// <summary>
        /// A list of all managed targets
        /// </summary>
        IReadOnlyList<ITarget> Targets { get; }

        /// <summary>
        /// Find and parse all managed target directories within <see cref="path"/>
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of all the found <see cref="Target"/>s.</returns>
        Task<List<ITarget>> LoadTargets();

        
        /// <summary>
        /// Configure a new target with the given disunity version or latest available version
        /// </summary>
        /// <param name="executablePath">The path to the target executable</param>
        /// <param name="disunityVersion">The desired disunity version to install in the default profile. Null for latest compatible version</param>
        /// <returns>The newly created <see cref="Target"/> or null</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<ITarget> InitTarget(string executablePath, string disunityVersion=null);

    }

}