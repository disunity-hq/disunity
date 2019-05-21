using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Disunity.Editor.Editors {

    internal class RuntimeAssemblyEditor : BaseSettingsAssetEditor<AssemblyDefinitionAsset> {

        protected override IEnumerable<AssemblyDefinitionAsset> Setting {
            get => _settings.RuntimeAssemblies;
            set => _settings.RuntimeAssemblies = value.ToArray();
        }

        public RuntimeAssemblyEditor(EditorWindow window, ExportSettings settings) : base(window, settings) { }

        public override string Label() {
            return "Scripts";
        }

        public override string Title() {
            return "Runtime Assemblies";
        }

        public override string Help() {
            return @"Add runtime assemblies here.

<b>Runtime Assemblies</b>

Runtime assemblies are loaded by Disunity <b>after</b> the game 
has started. This gives them the ability to interact with internal
game systems and data-structures.

Generally, any Unity MonoBehaviours or ScriptableObject classes you
write will be included in a runtime assembly.

Runtime assemblies are defined by a Unity Assembly Definition
Asset. These can be created using the 
<b>Assets -> Create -> Assembly Definition</b> menu.";
        }
    }
}