using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Disunity.Editor.Editors {

    internal class SceneEditor : BaseSettingsAssetEditor<SceneAsset> {

        protected override IEnumerable<SceneAsset> Setting {
            get => _settings.Scenes;
            set => _settings.Scenes = value.ToArray();
        }

        public SceneEditor(EditorWindow window, ExportSettings settings) : base(window, settings) { }

        public override string Label() {
            return "Scenes";
        }

        public override string Title() {
            return "Unity scenes";
        }

        public override string Help() {
            return "Scenes can be exported to your mod here.";
        }
    }
}