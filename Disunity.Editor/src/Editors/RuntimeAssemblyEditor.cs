using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    internal class RuntimeAssemblyEditor : BaseAssetEditor {

        public RuntimeAssemblyEditor(ExporterWindow window) : base(window) {
            _picker.Filters.Add(FilterPreloadAssemblies);
        }

        public override string GetAssetFilter() => "t:AssemblyDefinitionAsset";

        public override string Label() => "Runtime";

        public override string Title() => "Runtime Assemblies";

        public override string Help() => @"Add runtime assemblies here.

<b>Runtime Assemblies</b>

Runtime assemblies are loaded by Disunity <b>after</b> the game 
has started. This gives them the ability to interact with internal
game systems and data-structures.

Generally, any Unity MonoBehaviours or ScriptableObject classes you
write will be included in a runtime assembly.

Runtime assemblies are defined by a Unity Assembly Definition
Asset. These can be created using the 
<b>Assets -> Create -> Assembly Definition</b> menu.";

        public override string[] GetSelections() => _window.Settings.RuntimeAssemblies;

        public override void SelectionRemoved(string selection) {
            _window.Settings.RuntimeAssemblies = _window.Settings.RuntimeAssemblies.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.RuntimeAssemblies.ToList();
            var entry = selection as AssetEntry;
            list.Add(entry.Value);
            _window.Settings.RuntimeAssemblies = list.ToArray();
        }

        protected void FilterPreloadAssemblies(List<ListEntry> entries) {
            foreach (TreeEntry entry in entries) {
                if (_window.Settings.PreloadAssemblies.Contains(entry.Value)) {
                    entry.Enabled = false;
                }

                if (entry.Children != null)
                    FilterPreloadAssemblies(new List<ListEntry>(entry.Children));
            }
        }

    }
}