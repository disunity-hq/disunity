using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    class PreloadAssemblyEditor : BaseAssetEditor {

        public PreloadAssemblyEditor(ExporterWindow window) : base(window) {
            _picker.Filters.Add(FilterRuntimeAssemblies);
        }

        public override string GetAssetFilter() => "t:AssemblyDefinitionAsset";

        public override string Label() => "Preload";

        public override string Title() => "Preload Assemblies";

        public override string Help() => @"
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

        public override string[] GetSelections() => _window.Settings.PreloadAssemblies;

        public override void SelectionRemoved(string selection) {
            _window.Settings.PreloadAssemblies = _window.Settings.PreloadAssemblies.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.PreloadAssemblies.ToList();
            var entry = selection as AssetEntry;
            list.Add(entry.Value);
            _window.Settings.PreloadAssemblies = list.ToArray();
        }
        protected void FilterRuntimeAssemblies(List<ListEntry> entries) {
            foreach (TreeEntry entry in entries) {
                if (_window.Settings.RuntimeAssemblies.Contains(entry.Value)) {
                    entry.Enabled = false;
                }

                if (entry.Children != null)
                    FilterRuntimeAssemblies(new List<ListEntry>(entry.Children));
            }
        }

    }
}