using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    internal abstract class SelectionEditor<T> where T : Object {

        private Object _objectSelection;
        private List<Object> _selections;
        protected readonly ExportSettings Settings;

        protected SelectionEditor(ExportSettings settings) {
            Settings = settings;
        }

        public abstract List<Object> Selections { get; set; }
        public abstract bool ValidateCandidate(AssetPicker.HierarchyEntry candidate);
        public abstract void DrawHelpBox();

        private void DrawSelector() {
            EditorGUILayout.BeginHorizontal();

            PickerFields.AssetField<T>(_objectSelection, (o) => _objectSelection = o, ValidateCandidate);

            if (_objectSelection != null && GUILayout.Button("+", GUILayout.Width(24), GUILayout.Height(14))) {
                _selections.Add(_objectSelection);
                _objectSelection = null;
            }

            EditorGUILayout.EndHorizontal();
        }

        private bool DrawSelection(Object selection) {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(selection));

            var removed = GUILayout.Button("-", GUILayout.Width(24), GUILayout.Height(14));

            EditorGUILayout.EndHorizontal();

            return removed;
        }

        private void DrawSelections() {
            Object removed = null;

            foreach (var selection in _selections) {
                if (DrawSelection(selection)) {
                    removed = selection;
                }
            }

            if (removed != null) {
                _selections.Remove(removed);
            }
        }

        public void Draw() {
            _selections = Selections;

            DrawSelector();
            DrawHelpBox();
            DrawSelections();

            Selections = _selections;
        }

    }

}