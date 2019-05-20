using System.Linq;
using UnityEditor;


namespace Disunity.Editor.Editors {

    internal class SceneEditor : BaseAssetEditor {

        public SceneEditor(EditorWindow window, ExportSettings settings) : base(window) { }

        public override string Label() {
            return "Scenes";
        }

        public override string Title() {
            return "Unity scenes";
        }

        public override string Help() {
            return "Scenes can be exported to your mod here.";
        }
        public override string[] GetAssetPaths() {
            return AssetDatabase
                .FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
        }
    }
}