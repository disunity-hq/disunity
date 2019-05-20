using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;


namespace Disunity.Editor.Editors {

    class DependencyEditor : BaseSelectionEditor<VersionEntry, DependencyPicker> {

        public DependencyEditor(EditorWindow window) : base(window) { }

        public override string EntryAsString(VersionEntry selection) {
            return selection == null ? "null" : selection.Value.full_name;
        }

        public override void SelectionFiltered(ITreeEntry entry) {
            entry.Parent.SetEnabledRecursive(false);
            if (!entry.Parent.Parent.Children.Any(o => o.Enabled))
                entry.Parent.Parent.SetEnabledRecursive(false);
        }

        private static List<OwnerEntry> GenerateGraph(List<StoreClient.Dependency> dependencies) {
            var map = new Dictionary<string, OwnerEntry>();
            foreach (var dep in dependencies) {
                var owner = dep.owner;
                var ownerNode = map.ContainsKey(owner) ? map[owner] : new OwnerEntry() { value = dep };
                map[owner] = ownerNode;

                var modNode = new ModEntry() {
                    value = dep,
                    Expanded = false
                };

                foreach (var version in dep.versions) {
                    modNode.Add(new VersionEntry() {
                        value = version,
                        Expanded = false
                    });
                }

                ownerNode.Add(modNode);
            }

            return map.Values.ToList();
        }


        public override List<IEntry> Generator() {
            return new List<IEntry>(GenerateGraph(StoreClient.FetchDependencies()));
        }

        public override string Label() {
            return "Dependencies";
        }

        public override string Title() {
            return "Mod Dependencies";
        }

        public override string Help() {
            return @"Artifacts are unmanaged files copied into your mod folder.

Artifacts are useful for things like README files and other non-Unity files. They'll
be copied directly into the root of your mod directory.

To access your mod artifacts from code, use the `Mod.Info.Path` attribute to obtain
your mod's root path.";
        }
    }
}