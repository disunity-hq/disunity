using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal class SceneEditor : SelectionEditor<Object> {

        public SceneEditor(ExportSettings settings) : base(settings) { }

        public override void DrawHelpBox() {
            EditorGUILayout.HelpBox("Add scenes from your project to your mod.", MessageType.Info, true);
        }

        public override List<Object> Selections {
            get => Settings.Scenes.ToList();
            set => Settings.Scenes = value.ToArray();
        }

        public override bool ValidateCandidate(AssetPicker.HierarchyEntry candidate) {
            return candidate.Path.EndsWith(".unity");
        }
    }
}