using Disunity.Core;
using UnityEngine;


namespace Disunity.Editor {

    public class ExportSettings : ScriptableObject {
        [field: SerializeField] public int SelectedEditor { get; set; }

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
        [field: SerializeField] public string[] PreloadAssemblies { get; set; } = { };

        [field: SerializeField] public string[] RuntimeAssemblies { get; set; } = { };

        [field: SerializeField] public string[] Artifacts { get; set; } = { };
        [field: SerializeField] public string[] Prefabs { get; set; } = { };
        [field: SerializeField] public string[] Scenes { get; set; } = { };

        [field: SerializeField] public string OutputDirectory { get; set; }

        [field: SerializeField] public ContentType ContentTypes { get; set; }

        [field: SerializeField] public string RuntimeClass { get; set; }

        [field: SerializeField] public string RuntimeAssembly { get; set; }

        [field: SerializeField] public string PreloadClass { get; set; }

        [field: SerializeField] public string PreloadAssembly { get; set; }

        [field: SerializeField] public string[] Dependencies { get; set; }

    }

}