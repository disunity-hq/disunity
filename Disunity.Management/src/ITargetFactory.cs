using System.Collections.Generic;


namespace Disunity.Management {

    public interface ITargetFactory {

        /// <summary>
        /// Read a <code>target-info.json</code> and parse it into a <see cref="Target"/>
        /// </summary>
        /// <param name="path">The path to the <code>target-info.json</code></param>
        /// <returns>A parsed <see cref="Target"/> or <code>null</code> if the file doesn't exist</returns>
        Target FromFile(string path);

        /// <summary>
        /// Find and parse all managed target directories within <see cref="path"/>
        /// </summary>
        /// <remarks>
        /// Will search exactly one directory deep for <code>target-info.json</code>.
        /// Example:
        /// <code>
        /// managed
        /// └───risk-of-rain-2
        ///        │   target-info.json
        ///        │
        ///        └───foo
        ///            target-info.json
        /// </code>
        /// </remarks>
        /// <param name="path">The directory to search in</param>
        /// <returns>A <see cref="List{Target}"/> of all the found <see cref="Target"/>s.</returns>
        List<Target> LoadAllFromPath(string path);

        /// <summary>
        /// Setup a managed target directory and create the corresponding <see cref="Target"/>
        /// </summary>
        /// <param name="executablePath"></param>
        /// <param name="displayName"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        Target CreateManagedTarget(string executablePath, string displayName, string slug);

    }

}