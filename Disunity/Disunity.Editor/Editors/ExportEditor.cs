using System;
using System.Linq;
using Disunity.Editor.Fields;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Disunity.Editor.Editors {

    internal class ExportEditor : BaseEditor {

        private readonly ExportSettings _settings;
        private readonly ClassPickerField preloadPicker;
        private readonly ClassPickerField runtimePicker;

        public ExportEditor(EditorWindow window, ExportSettings settings) : base(window) {
            _settings = settings;
            preloadPicker = new ClassPickerField();
            runtimePicker = new ClassPickerField();
        }

        private string GetShortString(string str) {
            var maxWidth = (int) EditorGUIUtility.currentViewWidth - 252;
            var cutoffIndex = Mathf.Max(0, str.Length - 7 - maxWidth / 7);
            var shortString = str.Substring(cutoffIndex);
            if (cutoffIndex > 0) {
                shortString = "..." + shortString;
            }

            return shortString;
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
            settings.UpdateContentTypes();
            if (settings.ContentTypes == 0) {
                throw new ExportValidationError("You must include some content in your mod.");
            }
        }

        private void DrawStartupSelector(ClassPickerField field, AssemblyDefinitionAsset[] assemblies, string currentClass, string currentAssembly, Action<string, string> handler, string labelText) {
            if (assemblies.Length == 0) {
                return;
            }

            DrawSection(() => {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{labelText}:", GUILayout.Width(145));

                field.OnGUI(currentClass, currentAssembly, assemblies, handler);

                if (field.Selection != null) {
                    if (GUILayout.Button("X", GUILayout.Width(24), GUILayout.Height(14))) {
                        field.Selection = null;
                        handler(null, null);
                    }
                }

                EditorGUILayout.EndHorizontal();
            });
        }

        private void DrawDirectorySelector(ExportSettings settings) {
            GUILayout.BeginHorizontal();

            EditorGUILayout.TextField("Output Directory:", GetShortString(settings.OutputDirectory));

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
            DrawSection(() => {
                DrawDirectorySelector(settings);
            });
        }

        private void DrawSections(ExportSettings settings) {
            DrawDetails(settings);
            DrawStartupSelector(preloadPicker, settings.PreloadAssemblies, settings.PreloadClass, settings.PreloadAssembly, (c, a) => {
                settings.PreloadClass = c;
                settings.PreloadAssembly = a;
                preloadPicker.Picker.Close();
                _window.Repaint();
            }, "Preload class");
            DrawStartupSelector(runtimePicker, settings.RuntimeAssemblies, settings.RuntimeClass, settings.RuntimeAssembly, (c, a) => {
                settings.RuntimeClass = c;
                settings.RuntimeAssembly = a;
                runtimePicker.Picker.Close();
                _window.Repaint();
            }, "Runtime class");
            DrawExportOptions(settings);
        }

        public override string Label() {
            return "Export";
        }

        public override string Title() {
            return "Mod Export";
        }

        public override void Draw() {
            var valid = true;

            try {
                DrawSections(_settings);
            }
            catch (ExportValidationError e) {
                EditorGUILayout.HelpBox(e.Message, MessageType.Warning);
                valid = false;
            }

            if (valid && GUILayout.Button("Export")) {
                Export.ExportMod(_settings);
            }
        }

        public override string Help() {
            return @"Specify your mod's basic details and export.

Each mod <b>must</b>:
  - Provide all basic info
  - Include some content

Once you've provided all basic mod details and added at least some content
then the `Export` button will appear.";
        }

    }

}