using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Disunity.Editor {

    internal class RuntimeAssemblyEditor : SelectionEditor<AssemblyDefinitionAsset> {

        public override List<Object> Selections {
            get => Settings.RuntimeAssemblies.ToList();
            set => Settings.RuntimeAssemblies = value.ToArray();
        }

        public override bool ValidateCandidate(AssetPicker.HierarchyEntry candidate) {
            return Settings.RuntimeAssemblies.All(s => AssetDatabase.GetAssetPath(s) != candidate.Path) && 
                   Settings.PreloadAssemblies.All(s => AssetDatabase.GetAssetPath(s) != candidate.Path);
        }

        public override void DrawHelpBox() {
            EditorGUILayout.HelpBox("Add asmdefs from your project to be exported into your mod.", MessageType.Info, true);
        }

        public RuntimeAssemblyEditor(ExportSettings settings) : base(settings) { }

    }
}