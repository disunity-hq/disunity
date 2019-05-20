using System.Linq;
using UnityEditor;


namespace Disunity.Editor.Editors {

    internal class RuntimeAssemblyEditor : BaseAssetEditor {

        public RuntimeAssemblyEditor(EditorWindow window, ExportSettings settings) : base(window) { }

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

        public override string[] GetAssetPaths() {
            return AssetDatabase
                .FindAssets("t:AssemblyDefinitionAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
        }

    }
}