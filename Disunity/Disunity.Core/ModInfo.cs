using System.IO;
using Commons.Json;


namespace Disunity.Core {

    /// <summary>
    ///     Class that stores a Mod's name, author, description, version, path and supported platforms.
    /// </summary>
    public class ModInfo {

        private bool _isEnabled;
        private string _name;
        private ContentType _contentTypes;
        private string _author;
        private string _description;
        private string _version;
        private string _unityVersion;
        private string[] _preloadAssemblies;
        private string[] _runtimeAssemblies;
        private string _runtimeClass;
        private string _runtimeAssembly;
        private string _preloadClass;
        private string _preloadAssembly;

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
            ContentType contentTypes,
            string[] preloadAssemblies,
            string[] runtimeAssemblies,
            string preloadClass,
            string preloadAssembly,
            string runtimeClass,
            string runtimeAssembly) {

            _author = author;
            _description = description;
            _name = name;
            _contentTypes = contentTypes;
            _version = version;
            _unityVersion = unityVersion;
            _preloadAssemblies = preloadAssemblies;
            _runtimeAssemblies = runtimeAssemblies;
            _runtimeClass = runtimeClass;
            _runtimeAssembly = runtimeAssembly;
            _preloadClass = preloadClass;
            _preloadAssembly = preloadAssembly;

            _isEnabled = false;
        }

        public ModInfo() {

        }

        /// <summary>
        ///     Name
        /// </summary>
        
        public string Name {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        ///     The Mod's available content types.
        /// </summary>
        
        public int ContentTypes {
            get => (int)_contentTypes;
            set => _contentTypes = (ContentType)value;
        }

        /// <summary>
        ///     Mod author.
        /// </summary>
        
        public string Author {
            get => _author;
            set => _author = value;
        }

        /// <summary>
        ///     Mod description.
        /// </summary>
        
        public string Description {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        ///     Mod version.
        /// </summary>
        
        public string Version {
            get => _version;
            set => _version = value;
        }

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>
        
        public string UnityVersion {
            get => _unityVersion;
            set => _unityVersion = value;
        }


        public string[] PreloadAssemblies {
            get => _preloadAssemblies;
            set => _preloadAssemblies = value;
        }


        public string[] RuntimeAssemblies {
            get => _runtimeAssemblies;
            set => _runtimeAssemblies = value;
        }

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string RuntimeClass {
            get => _runtimeClass;
            set => _runtimeClass = value;
        }

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string RuntimeAssembly {
            get => _runtimeAssembly;
            set => _runtimeAssembly = value;
        }

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string PreloadClass {
            get => _preloadClass;
            set => _preloadClass = value;
        }

        /// <summary>
        ///     The version of Unity that was used to export this mod.
        /// </summary>

        public string PreloadAssembly {
            get => _preloadAssembly;
            set => _preloadAssembly = value;
        }

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
            var json = JsonMapper.ToJson(modInfo);
            File.WriteAllText(path, json);
        }

        /// <summary>
        ///     Load a ModInfo.
        /// </summary>
        /// <param name="path">The path to load the ModInfo from.</param>
        /// <returns>The loaded Modinfo, if succeeded. Null otherwise.</returns>
        public static ModInfo Load(string path) {
            path = System.IO.Path.GetFullPath(path);

            if (!File.Exists(path)) {
                return null;
            }

            var json = File.ReadAllText(path);

            var modInfo = JsonMapper.To<ModInfo>(json);

            modInfo.Path = path;

            return modInfo;

        }

    }

}