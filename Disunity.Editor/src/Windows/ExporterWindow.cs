using Disunity.Editor.Components;
using Disunity.Editor.Editors;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor.Windows {

    public class ExporterWindow : EditorWindow {

        private EditorSelector _editorSelector;
        private EditorScriptableSingleton<ExportSettings> _exportSettings;

        public ExportSettings Settings => _exportSettings.Instance;

        [MenuItem("Disunity/Exporter")]
        public static void ShowWindow() {
            var window = GetWindow<ExporterWindow>();
            window.titleContent = new GUIContent("Disunity Exporter");
            window.minSize = new Vector2(150, 150);
            window.Focus();
        }

        private void OnEnable() {
            _exportSettings = new EditorScriptableSingleton<ExportSettings>();
            _editorSelector = new EditorSelector(_exportSettings.Instance.SelectedEditor);

            _editorSelector.Add(new PrefabEditor(this));
            _editorSelector.Add(new SceneEditor(this));
            _editorSelector.Add(new ArtifactEditor(this));
            _editorSelector.Add(new PreloadAssemblyEditor(this));
            _editorSelector.Add(new RuntimeAssemblyEditor(this));
            _editorSelector.Add(new DependencyEditor(this));
            _editorSelector.Add(new ExportEditor(this));
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

            var myStyle = GUI.skin.GetStyle("HelpBox");
            myStyle.richText = true;

            EditorGUILayout.TextArea(message, myStyle);
        }

        private void OnGUI() {
            _exportSettings.Instance.SelectedEditor = _editorSelector.Draw();
        }

        public static void ExportMod() {
            var singleton = new EditorScriptableSingleton<ExportSettings>();
            Export.ExportMod(singleton.Instance);
        }

    }

}