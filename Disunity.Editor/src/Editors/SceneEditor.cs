using System.Linq;

using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    internal class SceneEditor : BaseAssetEditor {

        public SceneEditor(ExporterWindow window) : base(window) { }

        public override string GetAssetFilter() {
            return "t:Scene";
        }

        public override string Label() {
            return "Scenes";
        }

        public override string Title() {
            return "Unity scenes";
        }

        public override string Help() {
            return "Scenes can be exported to your mod here.";
        }

        public override string[] GetSelections() {
            return _window.Settings.Scenes;
        }

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