using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Management.Models;


namespace Disunity.Management.Factories {

    public interface ITargetFactory {

        /// <summary>
        /// Read a <code>target-info.json</code> and parse it into a <see cref="Target"/>
        /// </summary>
        /// <param name="path">The path to the <code>target-info.json</code></param>
        /// <returns>A parsed <see cref="Target"/> or <code>null</code> if the file doesn't exist</returns>
        Task<Target> FromFile(string path);

        
        /// <summary>
        /// Setup a managed target directory and create the corresponding <see cref="Target"/>
        /// </summary>
        /// <param name="executablePath"></param>
        /// <param name="displayName"></param>
        /// <param name="slug"></param>
        /// <param name="hashLength"></param>
        /// <returns></returns>
        Task<Target> CreateManagedTarget(string executablePath, string displayName, string slug, int? hashLength = null);

    }

}