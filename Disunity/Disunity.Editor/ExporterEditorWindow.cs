using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Disunity.Editor.Editors;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace Disunity.Editor {

    public class ExporterEditorWindow : EditorWindow {

        private ArtifactEditor _artifactEditor;
        private RuntimeAssemblyEditor _runtimeAssemblyEditor;
        private PreloadAssemblyEditor _preloadAssemblyEditor;
        private PrefabEditor _prefabEditor;
        private SceneEditor _sceneEditor;
        private EditorSelector _editorSelector;
        private DependencyEditor _dependencyEditor;

        private ExportEditor _exportEditor;
        private EditorScriptableSingleton<ExportSettings> _exportSettings;

        private int _selectedTab;

        [MenuItem("Disunity/Exporter")]
        public static void ShowWindow() {
            var window = GetWindow<ExporterEditorWindow>();
            window.titleContent = new GUIContent("Disunity Exporter");
            window.minSize = new Vector2(150, 150);
            window.Focus();
        }

        private void OnEnable() {
            _exportSettings = new EditorScriptableSingleton<ExportSettings>();
            _runtimeAssemblyEditor = new RuntimeAssemblyEditor(this, _exportSettings.Instance);
            _preloadAssemblyEditor = new PreloadAssemblyEditor(this, _exportSettings.Instance);
            _artifactEditor = new ArtifactEditor(this, _exportSettings.Instance);
            _prefabEditor = new PrefabEditor(this, _exportSettings.Instance);
            _sceneEditor = new SceneEditor(this, _exportSettings.Instance);
            _exportEditor = new ExportEditor(this, _exportSettings.Instance);
            _dependencyEditor = new DependencyEditor(this);
            _editorSelector = new EditorSelector();
            _editorSelector.AddEditor(_runtimeAssemblyEditor);
            _editorSelector.AddEditor(_prefabEditor);
            _editorSelector.AddEditor(_sceneEditor);
            _editorSelector.AddEditor(_preloadAssemblyEditor);
            _editorSelector.AddEditor(_dependencyEditor);
            _editorSelector.AddEditor(_artifactEditor);
            _editorSelector.AddEditor(_exportEditor);
        }

        private void DrawHelpInfo() {
            var message = @"<b>Disunity Exporter Quickhelp</b>

The full documentation is available at https://disunity.io/exporter/

<b>Exporting requires:</b>
    - All basic mod info specified
    - At least one kind of mod content is included

<b>Content types:</b>
    - Prefabs : premade gameobjects from your project
    - Scenes  : entire unity scenes to load in the game
    - Scripts : Assemblies of classes loaded at runtime
    - Preload : Assemblies of classes loaded before the game

<b>Assembly Definitions</b>

There are two kinds of mod assemblies. Runtime script assemblies, and
Preload assemblies. Runtime assemblies are loaded after the game, and 
can modify and hook game code, load prefabs and scenes and so on. 

Preload assemblies are loaded before the game, and can patch assemblies
before they loaded normally.

In both cases, assemblies are defined by Unity 'Assembly Definition Assets'
which can be created using the <b>Assets -> Create -> Assembly Definition</b> 
menu. Any script files in the same or child folders of the '.asmdef' file 
will be included in that assembly. 

Check the documentation for more information.
";

            GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
            myStyle.richText = true;

            EditorGUILayout.TextArea(message, myStyle);
        }

        private void OnGUI() {
            _editorSelector.Draw();
        }

        public static void ExportMod() {
            var singleton = new EditorScriptableSingleton<ExportSettings>();
            Export.ExportMod(singleton.Instance);
        }

    }

}