using System.Runtime.CompilerServices;
using Disunity.Shared;
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

        private ExportEditor _exportEditor;
        private EditorScriptableSingleton<ExportSettings> _exportSettings;

        private int _selectedTab;


        [MenuItem("Disunity/Export Mod")]
        public static void ShowWindow() {
            var window = GetWindow<ExporterEditorWindow>();
            window.titleContent = new GUIContent("Disunity Mod Export");
            window.minSize = new Vector2(450, 320);
            window.Focus();
        }

        private void OnEnable() {
            _exportSettings = new EditorScriptableSingleton<ExportSettings>();
            _runtimeAssemblyEditor = new RuntimeAssemblyEditor(_exportSettings.Instance);
            _preloadAssemblyEditor = new PreloadAssemblyEditor(_exportSettings.Instance);
            _artifactEditor = new ArtifactEditor(_exportSettings.Instance);
            _prefabEditor = new PrefabEditor(_exportSettings.Instance);
            _sceneEditor = new SceneEditor(_exportSettings.Instance);
            _exportEditor = new ExportEditor();
        }

        private void DrawExportEditor(ExportSettings settings) {
            if (_exportEditor.Draw(settings)) {
                var buttonPressed = GUILayout.Button("Export", GUILayout.Height(30));

                if (buttonPressed) {
                    Export.ExportMod(settings);
                }
            }
        }

        private void OnGUI() {
            GUI.enabled = !EditorApplication.isCompiling && !Application.isPlaying;

            var settings = _exportSettings.Instance;

            var tabs = new[] {"Export",  "Runtime Assemblies", "Prefabs", "Scenes", "Copy Artifacts", "Preload Assemblies"};

            _selectedTab = GUILayout.Toolbar(_selectedTab, tabs);

            switch (tabs[_selectedTab]) {
                case "Export":
                    DrawExportEditor(settings);
                    break;
                case "Runtime Assemblies":
                    _runtimeAssemblyEditor.Draw();
                    break;
                case "Preload Assemblies":
                    _preloadAssemblyEditor.Draw();
                    break;
                case "Copy Artifacts":
                    _artifactEditor.Draw();
                    break;
                case "Prefabs":
                    _prefabEditor.Draw();
                    break;
                case "Scenes":
                    _sceneEditor.Draw();
                    break;
            }
        }

        public static void ExportMod() {
            var singleton = new EditorScriptableSingleton<ExportSettings>();
            Export.ExportMod(singleton.Instance);
        }

    }

}