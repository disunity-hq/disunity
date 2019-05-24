using System.Linq;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    internal class PrefabEditor : BaseAssetEditor {

        public PrefabEditor(ExporterWindow window) : base(window) { }

        public override string GetAssetFilter() => "t:GameObject t:ScriptableObject";

        public override string Label() => "Prefabs";

        public override string Title() => "Premade GameObject prefabs";

        public override string Help() =>
            @"Prefabs and ScriptableObjects can be exported here.

You can access your prefabs via the `Mod.Prefabs` attribute. ScriptableObjects
work exactly the same way, except they can't be instantiated into the scene.

<b>ScriptableObjects</b>

ScriptableObjects are non-Scene-objects which can contain data. This is useful
for all sorts of things including settings. However, a limitation of Disunity
(and other mod solutions for Unity) is that you can only use basic data-types
for fields on a ScriptableObject. Normally, any custom C# class would work,
but due to a limitation in how Unity serializes information, only basic types
like strings, ints, floats, and basic arrays will work.";

        public override string[] GetSelections() => _window.Settings.Prefabs;

        public override void SelectionRemoved(string selection) {
            _window.Settings.Prefabs = _window.Settings.Prefabs.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.Prefabs.ToList();
            var entry = selection as AssetEntry;
            list.Add(entry.Value);
            _window.Settings.Prefabs = list.ToArray();
        }

    }
}