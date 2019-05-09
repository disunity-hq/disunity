using System.Collections.Generic;
using System.IO;


namespace Disunity.Core {

    /// <summary>
    ///     Class that represents a Mod.
    ///     A Mod lets you load scenes, prefabs and RuntimeAssemblies that have been exported.
    /// </summary>
    public abstract class Mod {
        /// <summary>
        /// Where the Mod is installed on disk.
        /// </summary>
        public string InstallPath { get; private set; }

        /// <summary>
        ///     This mod's Info.
        /// </summary>
        public ModInfo Info { get; }

        /// <summary>
        ///     Is the mod valid? A Mod becomes invalid when it is no longer being managed by the ModManager,
        ///     when any of its resources is missing or can't be loaded.
        /// </summary>
        public bool IsValid { get; protected set; }

        /// <summary>
        ///     Initialize a new Mod with a path to a mod file.
        /// </summary>
        /// <param name="infoPath">The path to a mod file</param>
        public Mod(string infoPath) {
            Info = ModInfo.Load(infoPath);
            InstallPath = Path.GetDirectoryName(infoPath);
        }
    }
}