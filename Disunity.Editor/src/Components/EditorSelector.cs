using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Editors;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Components {

    internal class EditorSelector {

        private readonly List<BaseEditor> _editors = new List<BaseEditor>();

        private int _selection;
        private bool _showHelp;

        private const string Message = @"Full documentation is available at:

  http://disunity.io/docs/

<b>Quickstart:</b>

Visit each of the tabs on the left to add content to your mod. 
Every mod must include some. The `Export` tab will allow you to
perform the export once some content has been added and all
basic mod details have been specified.

When selected, each tab contains a [?] button which further
explains how to use it.

";

        protected static GUIStyle _contentStyle = new GUIStyle() {
            margin = new RectOffset() {
                left = 5,
                top = 8,
                bottom = 3
            },
            stretchWidth = true
        };

        protected static GUIStyle _titleStyle = new GUIStyle() {
            margin = new RectOffset() {
                bottom = 10
            }
        };

        protected static GUIStyle _menuStyle = new GUIStyle() {
            margin = new RectOffset() {
                top = 8,
                left = 8
            }
        };

        public EditorSelector(int selectedEditor = 0) {
            _selection = selectedEditor;
        }

        protected void DrawContentTitle(string title, bool drawHelpButton = true) {
            using (new EditorGUILayout.HorizontalScope(_titleStyle)) {
                GUILayout.Label($"{title}:", new GUIStyle() { fontSize = 14 });

                if (drawHelpButton) {
                    _showHelp = GUILayout.Toggle(_showHelp, "?", "Button", GUILayout.Width(16));
                }
            }
        }

        protected void DrawMenu() {
            using (new EditorGUILayout.VerticalScope(_menuStyle, GUILayout.Width(105))) {
                var labels = _editors.Select(o => o.Label()).ToList();
                labels.Insert(0, "Help");
                _selection = GUILayout.SelectionGrid(_selection, labels.ToArray(), 1, GUILayout.Width(95));
            }
        }

        protected void DrawTitle(BaseEditor editor) {
            var title = editor.Title();
            DrawContentTitle(title);
        }

        protected void DrawHelp(BaseEditor editor) {
            var help = editor.Help();
            EditorGUILayout.HelpBox(help, MessageType.None, true);
        }

        protected void DrawMainHelp() {
            DrawContentTitle("Disunity Exporter", false);
            EditorGUILayout.HelpBox(Message, MessageType.None, true);
        }

        protected void DrawContent() {
            using (new EditorGUILayout.VerticalScope(_contentStyle, GUILayout.ExpandWidth(true))) {
                if (_selection == 0) {
                    DrawMainHelp();
                    return;
                }

                var selectedEditor = _editors[_selection - 1];

                DrawTitle(selectedEditor);

                if (_showHelp) {
                    DrawHelp(selectedEditor);
                } else {
                    selectedEditor.OnGUI();
                }
            }
        }

        public void Add(BaseEditor editor) => _editors.Add(editor);

        public int Draw() {

            var helpStyle = GUI.skin.GetStyle("HelpBox");
            helpStyle.richText = true;
            helpStyle.padding = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.BeginHorizontal();

            DrawMenu();
            DrawContent();

            EditorGUILayout.EndHorizontal();

            return _selection;
        }

    }

}