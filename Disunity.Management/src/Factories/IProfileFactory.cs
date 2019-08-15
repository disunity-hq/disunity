using System.Threading.Tasks;

using Disunity.Management.Models;


namespace Disunity.Management.Factories {

    public interface IProfileFactory {

        /// <summary>
        /// Loads the profile meta data from the specified profile path.
        /// </summary>
        /// <remarks>
        /// Do not include the <code>meta.json</code> in the path, it will be appended automatically
        /// </remarks>
        /// <param name="path">The profile directory to load the meta data from</param>
        /// <returns>The meta data for the specified profile, or null</returns>
        Task<Profile> Load(string path);

        /// <summary>
        /// Create a new profile within the specified root path
        /// </summary>
        /// <remarks>
        /// Will create a new directory within <see cref="path"/> with a unique name (ie a GUID)
        /// and create a file <code>meta.json</code> within the newly created profile
        /// </remarks>
        /// <param name="path">The root directory in which to create a new profiles</param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        /// /// <seealso cref="CreateExactPath"/>
        Task<Profile> Create(string path, string displayName);
        
        /// <summary>
        /// Create a new profile with the exact path <see cref="path"/>.
        /// </summary>
        /// <remarks>
        /// Will create a new directory <see cref="path"/> and create a file <code>meta.json</code> within the newly created profile
        /// </remarks>
        /// <param name="path">The directory in which to create a new profile</param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        /// <seealso cref="Create"/>
        Task<Profile> CreateExactPath(string path, string displayName);

    }

}