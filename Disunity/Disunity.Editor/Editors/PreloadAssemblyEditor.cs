using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Disunity.Editor.Editors {

    internal class PreloadAssemblyEditor : BaseSettingsAssetEditor<AssemblyDefinitionAsset> {

        protected override IEnumerable<AssemblyDefinitionAsset> Setting {
            get => _settings.PreloadAssemblies;
            set => _settings.PreloadAssemblies = value.ToArray();
        }

        public PreloadAssemblyEditor(EditorWindow window, ExportSettings settings) : base(window, settings) { }

        public override string Label() {
            return "Preload";
        }

        public override string Title() {
            return "Preload Assemblies";
        }

        public override string Help() {
            return @"
Preload assemblies are loaded by Disunity before the game, or
any of the assemblies it depends on, are loaded. This gives
Preload Assemblies an early-enough chance to modify those 
assemblies.

Generally, any Unity MonoBehaviours or ScriptableObject classes you
write <b>will not</b> be included in a preload assembly. Instead, 
use a runtime assembly for those.

Preload assemblies are defined by a Unity Assembly Definition
Asset. These can be created using the 
<b>Assets -> Create -> Assembly Definition</b> menu.
";
        }
    }
}