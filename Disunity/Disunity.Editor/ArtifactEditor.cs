using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal class ArtifactEditor : SelectionEditor<Object> {

        public ArtifactEditor(ExportSettings settings) : base(settings) { }

        public override void DrawHelpBox() {
            EditorGUILayout.HelpBox("Add files from your project to copy directly into your mod folder.", MessageType.Info, true);
        }

        public override List<Object> Selections {
            get => Settings.Artifacts.ToList();
            set => Settings.Artifacts = value.ToArray();
        }

        public override bool ValidateCandidate(AssetPicker.HierarchyEntry candidate) {
            return true;
        }
    }
}