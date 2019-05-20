using System.Collections.Generic;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Editors {
    abstract class BaseSelectionEditor<T, TP> : BaseEditor where T : class, IEntry where TP : FilteredPicker, new() {

        protected TP _pickerWindow;
        protected T _currentSelection;
        protected List<T> _currentSelections = new List<T>();
        protected GUIStyle _style;

        public abstract List<IEntry> Generator();

        public virtual GUIStyle GetSelectorStyle() {
            return new GUIStyle("TextField") { fixedHeight = 16 };
        }

        public virtual string EntryAsString(T selection) {
            return selection == null ? "null" : selection.AsString;
        }

        public virtual void DrawSelector() {
            if (GUILayout.Button(_currentSelection == null ? "None selected" : EntryAsString(_currentSelection), _style)) {
                _pickerWindow = EditorWindow.GetWindow<TP>();
                _pickerWindow.Set(Generator(), true);
                _pickerWindow.OnSelection += (s, o) => SelectionCallback(s, o as T);
                _pickerWindow.Filters = new FilterSet() {_pickerWindow.SearchFilter, SelectionFilter };
            }
        }

        public virtual void DrawSelection(T selection) {
            EditorGUILayout.LabelField(EntryAsString(selection));
        }

        public virtual void SelectionCallback(object sender, T selection) {
            _currentSelection = selection;
            _window.Repaint();
        }

        protected virtual void HandleAddition(T addition) {
            if (_pickerWindow != null) {
                _pickerWindow.Sort();
                _pickerWindow.Repaint();
            }
        }

        protected virtual void HandleSubtraction(T subtraction) {
            if (_pickerWindow != null) {
                _pickerWindow.Sort();
                _pickerWindow.Repaint();
            }
        }

        private bool DrawAddButton() {
            return GUILayout.Button("+", GUILayout.Width(24), GUILayout.Height(14));
        }
        private bool DrawRemoveButton() {
            return GUILayout.Button("-", GUILayout.Width(24), GUILayout.Height(14));
        }

        private void DrawSelectorRow() {
            EditorGUILayout.BeginHorizontal();

            DrawSelector();

            if (_currentSelection != null && DrawAddButton()) {
                _currentSelections.Add(_currentSelection);
                HandleAddition(_currentSelection);
                _currentSelection = null;
            }

            EditorGUILayout.EndHorizontal();
        }

        private bool DrawSelectionRow(T selection) {
            EditorGUILayout.BeginHorizontal();

            DrawSelection(selection);

            var removed = DrawRemoveButton();

            EditorGUILayout.EndHorizontal();

            return removed;
        }

        private void DrawSelections() {
            T removed = null;

            foreach (var selection in _currentSelections) {
                if (DrawSelectionRow(selection)) {
                    removed = selection;
                }
            }

            if (removed != null) {
                HandleSubtraction(removed);
                _currentSelections.Remove(removed);
            }
        }

        public override void Draw() {
            if (_style == null) {
                _style = GetSelectorStyle();
            }

            DrawSelectorRow();
            DrawSelections();
        }

        public virtual void SelectionFiltered(ITreeEntry entry) {
            entry.SetEnabledRecursive(false);
        }

        public virtual void SelectionFilter(List<IEntry> entries) {
            foreach (ITreeEntry entry in entries) {
                if (entry.Enabled == false) continue;
                if (entry.Children == null) {
                    foreach (var used in _currentSelections) {
                        if (used == entry) {
                            SelectionFiltered(entry);
                        }
                    }

                    continue;
                } 
                SelectionFilter(new List<IEntry>(entry.Children));
            }
        }

        protected BaseSelectionEditor(EditorWindow window) : base(window) {}
    }
}
