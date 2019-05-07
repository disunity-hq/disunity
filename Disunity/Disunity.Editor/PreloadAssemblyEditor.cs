using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;


namespace Disunity.Editor {

    internal class PreloadAssemblyEditor : SelectionEditor<AssemblyDefinitionAsset> {

        private const string Message = @"- Add asmdefs from your project to be exported into your mod as Preload Assemblies.
- Preload Assemblies are loaded before the game, its dependencies, or any mod Runtime Assemblies.
- Preload Assemblies are able to patch assemblies in ways not possible at runtime.";

        public PreloadAssemblyEditor(ExportSettings settings) : base(settings) { }

        public override void DrawHelpBox() {
            EditorGUILayout.HelpBox(Message, MessageType.Info, true);
        }

        public override List<Object> Selections {
            get => Settings.PreloadAssemblies.ToList();
            set => Settings.PreloadAssemblies = value.ToArray();
        }

        public override bool ValidateCandidate(AssetPicker.HierarchyEntry candidate) {
            return Settings.RuntimeAssemblies.All(s => AssetDatabase.GetAssetPath(s) != candidate.Path) && 
                   Settings.PreloadAssemblies.All(s => AssetDatabase.GetAssetPath(s) != candidate.Path);

        }
    }
}