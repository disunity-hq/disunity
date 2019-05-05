using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Disunity.Cecil;
using Disunity.Shared;
using Disunity.Interface;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal class ExportEditor {

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
                settings.Name = EditorGUILayout.TextField("Mod name:", settings.Name);
                settings.Author = EditorGUILayout.TextField("Author:", settings.Author);
                settings.Version = EditorGUILayout.TextField("Version:", settings.Version);
                settings.Description = EditorGUILayout.TextField("Description:", settings.Description, GUILayout.Height(60));
            });

            var details = new[] {settings.Name, settings.Author, settings.Version, settings.Description};

            if (details.Any(o => o == "")) {
                throw new ExportValidationError("All mod details must be specified.");
            }
        }

        private void DrawContentSelector(ExportSettings settings) {
            if (settings.PreloadAssemblies.Length > 0)
                settings.ContentTypes |= ContentType.PreloadAssemblies;
            if (settings.RuntimeAssemblies.Length > 0)
                settings.ContentTypes |= ContentType.RuntimeAssemblies;
            if (settings.Prefabs.Length > 0)
                settings.ContentTypes |= ContentType.Prefabs;
            if (settings.Scenes.Length > 0)
                settings.ContentTypes |= ContentType.Scenes;

            if ((int) settings.ContentTypes == 0) {
                throw new ExportValidationError("You must include some content in your mod.");
            }
        }

        private Dictionary<string, Type> GetModBehaviours(ExportSettings settings) {
            var modBehaviours = new Dictionary<string, Type>();

            foreach (var type in AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ModBehaviour))) {
                var assemblyName = type.Assembly.GetName().Name;
                var identifier = $"{assemblyName}.{type.FullName}";
                if (settings.StartupClass != type.FullName && settings.StartupAssembly != assemblyName)
                    modBehaviours[identifier] = type;
            }

            return modBehaviours;
        }

        private void DrawStartupSelector(ExportSettings settings) {
            if (settings.RuntimeAssemblies.Length == 0) {
                settings.StartupClass = null;
                settings.StartupAssembly = null;
                return;
            }

            DrawSection(() => {
                var modBehaviours = new Dictionary<string, Type>();

                var style = new GUIStyle("TextField") { fixedHeight = 16 };

                var label = string.IsNullOrEmpty(settings.StartupClass) ? 
                    "None Selected." : $"{settings.StartupAssembly}.{settings.StartupClass}";

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Startup ModBehaviour:", GUILayout.Width(145));

                StringPicker.Button(label, () => {
                    modBehaviours = GetModBehaviours(settings);
                    return modBehaviours.Keys;
                }, t => {
                    var behaviour = modBehaviours[t];
                    settings.StartupClass = behaviour.FullName;
                    settings.StartupAssembly = behaviour.Assembly.GetName().Name;
                }, false, style);

                if (settings.StartupClass != null) {
                    if (GUILayout.Button("X", GUILayout.Width(24), GUILayout.Height(14))) {
                        settings.StartupClass = null;
                        settings.StartupAssembly = null;
                    }
                }

                EditorGUILayout.EndHorizontal();
            });
        }

        private void DrawDirectorySelector(ExportSettings settings) {
            GUILayout.BeginHorizontal();

            EditorGUILayout.TextField("Output Directory*:", GetShortString(settings.OutputDirectory));

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

        private void DrawLogSelector() {
            LogUtility.LogLevel = (LogLevel) EditorGUILayout.EnumPopup("Log Level:", LogUtility.LogLevel);
        }

        private void DrawExportOptions(ExportSettings settings) {
            DrawSection(() => {
                DrawLogSelector();
                DrawDirectorySelector(settings);
            });
        }

        private void DrawSections(ExportSettings settings) {
            DrawDetails(settings);
            DrawStartupSelector(settings);
            DrawExportOptions(settings);
        }

        public bool Draw(ExportSettings settings) {
            var valid = true;

            try {
                DrawSections(settings);
            }
            catch (ExportValidationError e) {
                EditorGUILayout.HelpBox(e.Message, MessageType.Warning);
                valid = false;
            }

            return valid;
        }

    }

}