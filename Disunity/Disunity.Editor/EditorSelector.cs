using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Editors;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal class EditorSelector {

        private readonly List<BaseEditor> _editors = new List<BaseEditor>();

        private int _selection;
        private bool _showHelp;

        private string _message = @"The full documentation is available at:

  http://disunity.io/docs/

<b>Overview:</b>

Visit each of the tabs on the left to add content to your mod. 
Every mod must include some. The `Export` tab will allow you to
perform the export once some content has been added and all
basic mod details have been specified.

When selected, each tab contains a [?] button which further
explains how to use it.

";

        public EditorSelector() {

        }

        public void AddEditor(BaseEditor editor) {
            _editors.Add(editor);
        }

        private void BeginContentVertical() {
            EditorGUILayout.BeginVertical(
                new GUIStyle() {  margin = new RectOffset() { left = 5, top = 8 }, stretchWidth = true },
                GUILayout.ExpandWidth(true));
        }

        private void BeginTitleHorizontal() {
            EditorGUILayout.BeginHorizontal(
                new GUIStyle() { margin = new RectOffset() { bottom = 10 } });
        }

        private void DrawContentTitle(string title, bool drawHelpButton = true) {
            BeginTitleHorizontal();

            GUILayout.Label($"{title}:", new GUIStyle() { fontSize = 14 });

            if (drawHelpButton) {
                _showHelp = GUILayout.Toggle(_showHelp, "?", "Button", GUILayout.Width(16));
            }

            GUILayout.EndHorizontal();
        }

        private void DrawMenu() {
            EditorGUILayout.BeginVertical(
                new GUIStyle() { margin = new RectOffset() {top = 8} },
                GUILayout.Width(105));

            var labels = _editors.Select(o => o.Label()).ToList();
            labels.Insert(0, "Help");
            _selection = GUILayout.SelectionGrid(_selection, labels.ToArray(), 1, GUILayout.Width(95));

            EditorGUILayout.EndVertical();
        }

        private void DrawTitle(BaseEditor editor) {
            var title = editor.Title();
            DrawContentTitle(title);
        }

        private void DrawHelp(BaseEditor editor) {
            var help = editor.Help();
            EditorGUILayout.HelpBox(help, MessageType.None, true);
        }

        private void DrawMainHelp() {
            DrawContentTitle("Disunity Exporter", false);
            EditorGUILayout.HelpBox(_message, MessageType.None, true);
            EditorGUILayout.EndVertical();
        }

        private void DrawContent() {
            BeginContentVertical();

            if (_selection == 0) {
                DrawMainHelp();
                return;
            }

            var selectedEditor = _editors[_selection - 1];

            DrawTitle(selectedEditor);

            if (_showHelp) {
                DrawHelp(selectedEditor);
            }
            else {
                selectedEditor.Draw();
            }

            EditorGUILayout.EndVertical();
        }

        public void Draw() {

            GUIStyle helpStyle = GUI.skin.GetStyle("HelpBox");
            helpStyle.richText = true;
            helpStyle.padding = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.BeginHorizontal();

            DrawMenu();
            DrawContent();

            EditorGUILayout.EndHorizontal();
        }

    }

}