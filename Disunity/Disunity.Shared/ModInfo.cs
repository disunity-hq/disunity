using System;
using System.IO;
using UnityEngine;


namespace Disunity.Shared {

    /// <summary>
    ///     Class that stores a Mod's name, author, description, version, path and supported platforms.
    /// </summary>
    [Serializable]
    public class ModInfo {

        [SerializeField] private bool _isEnabled;
        [SerializeField] private string _name;
        [SerializeField] private ModPlatform _platforms;
        [SerializeField] private ContentType _contentTypes;
        [SerializeField] private string _author;
        [SerializeField] private string _description;
        [SerializeField] private string _version;
        [SerializeField] private string _unityVersion;
        [SerializeField] private string[] _preloadAssemblies;
        [SerializeField] private string[] _runtimeAssemblies;
        [SerializeField] private string _startupClass;
        [SerializeField] private string _startupAssembly;

        /// <summary>
        ///     Initialize a new ModInfo.
        /// </summary>
        /// <param name="name">The Mod's name.</param>
        /// <param name="author">The Mod's author.</param>
        /// <param name="description">The Mod's description.</param>
        /// <param name="platforms">The Mod's supported platforms.</param>
        /// <param name="content">The Mod's available content types.</param>
        /// <param name="version">The Mod's version</param>
        /// <param name="unityVersion"> The version of Unity that the Mod was exported with.</param>
        public ModInfo(
            string name,
            string author,
            string description,
            string version,
            string unityVersion,
            ModPlatform platforms,
            ContentType contentTypes,
            string[] preloadAssemblies,
            string[] runtimeAssemblies, 
            string startupClass, 
            string startupAssembly) {

            _author = author;
            _description = description;
            _name = name;
            _platforms = platforms;
            _contentTypes = contentTypes;
            _version = version;
            _unityVersion = unityVersion;
            _preloadAssemblies = preloadAssemblies;
            _runtimeAssemblies = runtimeAssemblies;
            _startupClass = startupClass;
            _startupAssembly = startupAssembly;

            _isEnabled = false;
        }

        /// <summary>
        ///     Name
        /// </summary>
        
        public string Name => _name;

        /// <summary>
        ///     Supported platforms for this mod.
        /// </summary>
        
        public ModPlatform Platforms => _platforms;

        /// <summary>
        ///     The Mod's available content types.
        /// </summary>
        
        public ContentType ContentTypes => _contentTypes;

        /// <summary>
        ///     Mod author.
        /// </summary>
        
        public string Author => _author;

        /// <summary>
        ///     Mod description.
        /// </summary>
        
        public string Description => _description;

        /// <summary>
        ///     Mod version.
        /// </summary>
        
        public string Version => _version;

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>
        
        public string UnityVersion => _unityVersion;


        
        public string[] PreloadAssemblies => _preloadAssemblies;


        
        public string[] RuntimeAssemblies => _runtimeAssemblies;

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string StartupClass => _startupClass;

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string StartupAssembly => _startupAssembly;

        /// <summary>
        ///     Should this mod be enabled.
        /// </summary>
        public bool IsEnabled {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        /// <summary>
        ///     Location of mod
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        ///     Save this ModInfo.
        /// </summary>
        public void Save() {
            if (!string.IsNullOrEmpty(Path)) {
                Save(Path, this);
            }
        }

        /// <summary>
        ///     Save a ModInfo.
        /// </summary>
        /// <param name="path">The path to save the ModInfo to.</param>
        /// <param name="modInfo">The ModInfo to save.</param>
        public static void Save(string path, ModInfo modInfo) {
            var json = JsonUtility.ToJson(modInfo, true);
            Debug.Log("JSON:");
            Debug.Log(json);

            File.WriteAllText(path, json);
        }

        /// <summary>
        ///     Load a ModInfo.
        /// </summary>
        /// <param name="path">The path to load the ModInfo from.</param>
        /// <returns>The loaded Modinfo, if succeeded. Null otherwise.</returns>
        public static ModInfo Load(string path) {
            path = System.IO.Path.GetFullPath(path);

            if (File.Exists(path)) {
                try {
                    var json = File.ReadAllText(path);

                    var modInfo = JsonUtility.FromJson<ModInfo>(json);

                    modInfo.Path = path;

                    return modInfo;
                }
                catch (Exception e) {
                    LogUtility.LogWarning("There was an issue while loading the ModInfo from " + path + " - " + e.Message);
                }
            }

            return null;
        }

    }

}