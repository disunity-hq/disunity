using Disunity.Core;
using UnityEngine;


namespace Disunity.Editor {

    public class ExportSettings : ScriptableObject {

        /// <summary>
        ///     The Mod's name.
        /// </summary>
        [field: SerializeField] public string Name { get; set; }

        /// <summary>
        ///     The Mod's author.
        /// </summary>
        [field: SerializeField] public string Author { get; set; }

        /// <summary>
        ///     The Mod's description.
        /// </summary>
        [field: SerializeField] public string Description { get; set; }

        /// <summary>
        ///     The Mod's version.
        /// </summary>
        [field: SerializeField] public string Version { get; set; }

        /// <summary>
        ///     The directory to which the Mod will be exported.
        /// </summary>
        [field: SerializeField] public Object[] PreloadAssemblies { get; set; } = { };

        [field: SerializeField] public Object[] RuntimeAssemblies { get; set; } = { };

        [field: SerializeField] public Object[] Artifacts { get; set; } = { };
        [field: SerializeField] public Object[] Prefabs { get; set; } = { };
        [field: SerializeField] public Object[] Scenes { get; set; } = { };

        [field: SerializeField] public string OutputDirectory { get; set; }

        [field: SerializeField] public ContentType ContentTypes { get; set; }

        [field: SerializeField] public string RuntimeClass { get; set; }

        [field: SerializeField] public string RuntimeAssembly { get; set; }

        [field: SerializeField] public string PreloadClass { get; set; }

        [field: SerializeField] public string PreloadAssembly { get; set; }

        public void UpdateContentTypes() {
            ContentTypes = 0;
            if (PreloadAssemblies.Length > 0) {
                ContentTypes |= ContentType.PreloadAssemblies;
            }
            if (RuntimeAssemblies.Length > 0) {
                ContentTypes |= ContentType.RuntimeAssemblies;
            }
            if (Prefabs.Length > 0) {
                ContentTypes |= ContentType.Prefabs;
            }
            if (Scenes.Length > 0) {
                ContentTypes |= ContentType.Scenes;
            }
            if (Artifacts.Length > 0) {
                ContentTypes |= ContentType.Artifacts;
            }
        }

    }

}