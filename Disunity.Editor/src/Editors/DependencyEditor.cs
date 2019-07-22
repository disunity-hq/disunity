using System.Collections.Generic;
using System.Linq;

using Disunity.Editor.Components;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;

using static Disunity.Editor.StoreClient;


namespace Disunity.Editor.Editors {

    internal class DependencyEditor : BaseSelectionEditor {

        public DependencyEditor(ExporterWindow window, FilteredPicker picker = null, Lister lister = null) : base(window, picker, lister) { }

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

        public override void Init() {
            _picker.Filters.Add(FilterOtherVersions);
            base.Init();
        }

        public void FilterOtherVersions(List<ListEntry> entries) {
            var selections = _window.Settings;

            foreach (TreeEntry entry in entries) {
                switch (entry) {
                    case VersionEntry versionEntry:
                        if (_window.Settings.Dependencies.Contains(versionEntry.Version.full_name)) {
                            versionEntry.Parent.SetEnabledRecursive(false);
                        }

                        break;

                    default:
                        if (entry.Children != null) {
                            FilterOtherVersions(new List<ListEntry>(entry.Children));
                        }

                        break;
                }
            }
        }

        private static List<OwnerListEntry> GenerateGraph(List<Dependency> dependencies) {
            var map = new Dictionary<string, OwnerListEntry>();

            foreach (var dep in dependencies) {
                var owner = dep.owner;
                var ownerNode = map.ContainsKey(owner) ? map[owner] : new OwnerListEntry() {Dependency = dep};
                map[owner] = ownerNode;

                var modNode = new ModEntry() {
                    Dependency = dep,
                    Expanded = false
                };

                foreach (var version in dep.versions) {
                    modNode.Add(new VersionEntry() {
                        Version = version,
                        Value = version.full_name,
                        Expanded = false
                    });
                }

                ownerNode.Add(modNode);
            }

            return map.Values.ToList();
        }

        public override List<ListEntry> GenerateOptions() {
            var dependencies = FetchDependencies();
            var graph = GenerateGraph(dependencies);
            return new List<ListEntry>(graph);
        }

        public override string[] GetSelections() {
            return _window.Settings.Dependencies;
        }

        public override void SelectionRemoved(string selection) {
            _window.Settings.Dependencies = _window.Settings.Dependencies.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.Dependencies.ToList();
            var entry = selection as VersionEntry;
            list.Add(entry.Value);
            _window.Settings.Dependencies = list.ToArray();
        }

        public class DependencyEntry : TreeEntry {

            public Dependency Dependency { get; set; }

        }

        public class OwnerListEntry : DependencyEntry {

            public override string ToString() {
                return Dependency.owner;
            }

        }

        public class ModEntry : DependencyEntry {

            public override string ToString() {
                return Dependency.name;
            }

        }

        public class VersionEntry : TreeEntry {

            public DependencyVersion Version { get; set; }

            public override string ToString() {
                return Version.version_number;
            }

        }

    }

}