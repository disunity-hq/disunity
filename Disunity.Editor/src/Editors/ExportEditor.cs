using System;
using System.Linq;

using Disunity.Core;
using Disunity.Editor.Fields;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor.Editors {

    internal class ExportEditor : BaseEditor {

        protected readonly ClassPickerField _preloadPickerField;
        protected readonly ClassPickerField _runtimePickerField;

        public ExportEditor(ExporterWindow window) : base(window) {
            _preloadPickerField = new PreloadPickerField(window);
            _runtimePickerField = new RuntimePickerField(window);
        }

        public override string Label() {
            return "Export";
        }

        public override string Title() {
            return "Mod Export";
        }

        public override string Help() {
            return @"Specify your mod's basic details and export.

Each mod <b>must</b>:
  - Provide all basic info
  - Include some content

Once you've provided all basic mod details and added at least some content
then the `Export` button will appear.";
        }

        private void DrawSection(Action thunk) {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true));

            GUILayout.Space(5);

            try {
                thunk();
                GUILayout.Space(5);
                EditorGUILayout.EndVertical();
            }
            catch (ExportValidationError) {
                GUILayout.Space(5);
                EditorGUILayout.EndVertical();
                throw;
            }
        }

        private void DrawDetails(ExportSettings settings) {
            DrawSection(() => {
                settings.Name = EditorGUILayout.TextField(new GUIContent("Mod name:", "Your mod's public name"), settings.Name);
                settings.Author = EditorGUILayout.TextField(new GUIContent("Author:", "Who should get credit for this mod?"), settings.Author);
                settings.Version = EditorGUILayout.TextField("Version:", settings.Version);
                settings.Description = EditorGUILayout.TextField(new GUIContent("Description:", "Short sentence describing what this mod does."), settings.Description, GUILayout.Height(60));
            });

            var details = new[] {settings.Name, settings.Author, settings.Version, settings.Description};

            if (details.Any(o => o == "")) {
                throw new ExportValidationError("All mod details must be specified.");
            }

            DrawContentWarning(settings);
        }

        private void DrawContentWarning(ExportSettings settings) {

            settings.ContentTypes = 0;

            if (settings.PreloadAssemblies.Length > 0) {
                settings.ContentTypes |= ContentType.PreloadAssemblies;
            }

            if (settings.RuntimeAssemblies.Length > 0) {
                settings.ContentTypes |= ContentType.RuntimeAssemblies;
            }

            if (settings.Prefabs.Length > 0) {
                settings.ContentTypes |= ContentType.Prefabs;
            }

            if (settings.Scenes.Length > 0) {
                settings.ContentTypes |= ContentType.Scenes;
            }

            if (settings.Artifacts.Length > 0) {
                settings.ContentTypes |= ContentType.Artifacts;
            }

            if (settings.ContentTypes == 0) {
                throw new ExportValidationError("You must include some content in your mod.");
            }
        }

        private void DrawStartupSelector(ClassPickerField field, string[] assemblies, string labelText) {
            if (assemblies.Length == 0) {
                return;
            }

            DrawSection(() => {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{labelText}:", GUILayout.Width(145));

                field.OnGUI();

                if (field.CurrentSelection() != null) {
                    if (GUILayout.Button("X", GUILayout.Width(24), GUILayout.Height(14))) {
                        field.SelectionMade(new ListEntry() {Value = null});
                    }
                }

                EditorGUILayout.EndHorizontal();
            });
        }

        private void DrawDirectorySelector(ExportSettings settings) {
            GUILayout.BeginHorizontal();

            EditorGUILayout.TextField("Output Directory:", settings.OutputDirectory);

            if (GUILayout.Button("...", GUILayout.Width(30))) {
                var selectedDirectory =
                    EditorUtility.SaveFolderPanel("Choose output directory", settings.OutputDirectory, "");

                if (!string.IsNullOrEmpty(selectedDirectory)) {
                    settings.OutputDirectory = selectedDirectory;
                }
            }

            GUILayout.EndHorizontal();

            if (settings.OutputDirectory == "") {
                throw new ExportValidationError("You must specify an output directory.");
            }
        }

        private void DrawExportOptions(ExportSettings settings) {
            DrawSection(() => { DrawDirectorySelector(settings); });
        }

        private void DrawSections(ExportSettings settings) {
            DrawDetails(settings);
            DrawStartupSelector(_preloadPickerField, settings.PreloadAssemblies, "Preload class");
            DrawStartupSelector(_runtimePickerField, settings.RuntimeAssemblies, "Runtime class");
            DrawExportOptions(settings);
        }

        public override void Draw() {
            var valid = true;

            try {
                DrawSections(_window.Settings);
            }
            catch (ExportValidationError e) {
                EditorGUILayout.HelpBox(e.Message, MessageType.Warning);
                valid = false;
            }

            if (valid && GUILayout.Button("Export")) {
                Export.ExportMod(_window.Settings);
            }
        }

    }

}