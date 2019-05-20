using UnityEditor;


namespace Disunity.Editor.Editors {

    internal class ArtifactEditor : BaseAssetEditor {

        public ArtifactEditor(EditorWindow window, ExportSettings settings) : base(window) { }

        public override string Label() {
            return "Artifacts";
        }

        public override string Title() {
            return "Copy Artifacts";
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