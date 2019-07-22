using System.Linq;
using Disunity.Editor.Components;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;


namespace Disunity.Editor.Editors {

    internal class ArtifactEditor : BaseAssetEditor {

        public class ArtifactLister : Lister {

            protected override void DrawItem(string item) {
                if (item.StartsWith("Assets/"))
                    item = item.Substring(7);
                EditorGUILayout.LabelField(item, GUILayout.ExpandWidth(true));
            }

        }

        public ArtifactEditor(ExporterWindow window) : base(window) { }

        public override string Label() => "Artifacts";

        public override string Title() => "Copy Artifacts";

        public override string Help() =>
            @"Artifacts are unmanaged files copied into your mod folder.

Artifacts are useful for things like README files and other non-Unity files. They'll
be copied directly into the root of your mod directory.

To access your mod artifacts from code, use the `Mod.Info.Path` attribute to obtain
your mod's root path.";
        public override Lister DefaultLister() {
            return new ArtifactLister();
        }

        public override string[] GetSelections() => _window.Settings.Artifacts;

        public override void SelectionRemoved(string selection) {
            _window.Settings.Artifacts = _window.Settings.Artifacts.Where(o => o != selection).ToArray();
        }

        public override void SelectionAdded(ListEntry selection) {
            var list = _window.Settings.Artifacts.ToList();
            var entry = selection as AssetEntry;
            list.Add(entry.Value);
            _window.Settings.Artifacts = list.ToArray();
        }

    }
}