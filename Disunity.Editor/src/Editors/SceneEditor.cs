using System.Linq;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    internal class SceneEditor : BaseAssetEditor {

        public SceneEditor(ExporterWindow window) : base(window) { }

        public override string GetAssetFilter() => "t:Scene";

        public override string Label() => "Scenes";

        public override string Title() => "Unity scenes";

        public override string Help() => "Scenes can be exported to your mod here.";

        public override string[] GetSelections() => _window.Settings.Scenes;

        public override void SelectionRemoved(string selection) {
            _window.Settings.Scenes = _window.Settings.Scenes.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.Scenes.ToList();
            var entry = selection as AssetEntry;
            list.Add(entry.Value);
            _window.Settings.Scenes = list.ToArray();
        }

    }
}