using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal class PrefabEditor : SelectionEditor<Object> {

        public PrefabEditor(ExportSettings settings) : base(settings) { }

        public override void DrawHelpBox() {
            EditorGUILayout.HelpBox("Add prefabs from your project to your mod.", MessageType.Info, true);
        }

        public override List<Object> Selections {
            get => Settings.Prefabs.ToList();
            set => Settings.Prefabs = value.ToArray();
        }

        public override bool ValidateCandidate(AssetPicker.HierarchyEntry candidate) {
            return candidate.Path.EndsWith(".prefab") || candidate.Path.EndsWith(".asset");
        }
    }
}