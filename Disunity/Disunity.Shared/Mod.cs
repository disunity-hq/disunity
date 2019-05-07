using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace Disunity.Core {

    /// <summary>
    ///     Class that represents a Mod.
    ///     A Mod lets you load scenes, prefabs and RuntimeAssemblies that have been exported.
    /// </summary>
    public abstract class Mod {

        private List<Mod> _conflictingMods = new List<Mod>();

        /// <summary>
        /// Where the Mod is installed on disk.
        /// </summary>
        public string InstallPath { get; private set; }

        /// <summary>
        ///     Collection of Mods that are in conflict with this Mod.
        /// </summary>
        public ReadOnlyCollection<Mod> ConflictingMods { get; private set; }

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
        ///     Set the mod to be enabled or disabled
        /// </summary>
        public bool IsEnabled {
            get => Info.IsEnabled;
            set {
                Info.IsEnabled = value;
                Info.Save();
            }
        }

        /// <summary>
        ///     Initialize a new Mod with a path to a mod file.
        /// </summary>
        /// <param name="infoPath">The path to a mod file</param>
        public Mod(string infoPath) {
            Info = ModInfo.Load(infoPath);
            InstallPath = Path.GetDirectoryName(infoPath);
            ConflictingMods = _conflictingMods.AsReadOnly();
            IsValid = true;
        }

        /// <summary>
        ///     Update this Mod's conflicting Mods with the supplied Mod
        /// </summary>
        /// <param name="other">Another Mod</param>
        //public void UpdateConflicts(Mod other) {
        //    if (other == this || !IsValid) {
        //        return;
        //    }

        //    if (!other.IsValid) {
        //        if (_conflictingMods.Contains(other)) {
        //            _conflictingMods.Remove(other);
        //        }

        //        return;
        //    }

        //    foreach (var assemblyName in _runtimeAssemblyNames)
        //    foreach (var otherAssemblyName in other.RuntimeAssemblyNames) {
        //        if (assemblyName == otherAssemblyName) {
        //            Debug.Log("Assembly " + other.Name + "/" + otherAssemblyName + " conflicting with " + Name + "/" +
        //                      assemblyName);

        //            if (!_conflictingMods.Contains(other)) {
        //                _conflictingMods.Add(other);
        //                return;
        //            }
        //        }
        //    }

        //    foreach (var sceneName in SceneNames)
        //    foreach (var otherSceneName in other.SceneNames) {
        //        if (sceneName == otherSceneName) {
        //            Debug.Log("Scene " + other.Name + "/" + otherSceneName + " conflicting with " + Name + "/" +
        //                      sceneName);

        //            if (!_conflictingMods.Contains(other)) {
        //                _conflictingMods.Add(other);
        //                return;
        //            }
        //        }
        //    }
        //}

        //public abstract void UpdateConflicts(Mod other);

        /// <summary>
        ///     Update this Mod's conflicting Mods with the supplied Mods
        /// </summary>
        /// <param name="mods">A collection of Mods</param>
        //public void UpdateConflicts(IEnumerable<Mod> mods) {
        //    //foreach (var mod in mods) {
        //    //    UpdateConflicts(mod);
        //    //}
        //}

        /// <summary>
        ///     Is another conflicting Mod loaded?
        /// </summary>
        /// <returns>True if another conflicting mod is loaded</returns>
        //public bool ConflictingModsLoaded() {
        //    return _conflictingMods.Any(m => m.IsValid);
        //}

        /// <summary>
        ///     Is another conflicting Mod enabled?
        /// </summary>
        /// <returns>True if another conflicting mod is enabled</returns>
        //public bool ConflictingModsEnabled() {
        //    return _conflictingMods.Any(m => m.IsEnabled);
        //}

        /// <summary>
        ///     Invalidate the mod
        /// </summary>
        public void SetInvalid() {
            IsValid = false;
        }

    }

}