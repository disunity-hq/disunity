using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor.Pickers {

    public class ListEntry : IComparable {

        public string Value;
        public bool Enabled { get; set; }

        public int CompareTo(object obj) {
            if (obj is ListEntry val) {
                return string.Compare(ToString(), val.ToString(), StringComparison.Ordinal);
            }

            return -1;
        }

        public override string ToString() {
            return Value;
        }

    }

    public class BasePicker {

        protected static GUIStyle _selectedStyle;
        protected static GUIStyle _unselectedStyle;
        protected double _lastClickTime;

        protected Vector2 _scrollPosition;

        protected ListEntry _selected;

        public List<ListEntry> Entries;

        public Func<List<ListEntry>> OptionGenerator;
        public Action<ListEntry> SelectionHandler;

        public bool NeedsDoubleClick { get; set; }
        public int EntriesShown { get; protected set; }

        protected virtual GUIStyle GetEntryStyle(ListEntry entry) {
            return entry == _selected ? _selectedStyle : _unselectedStyle;
        }

        public virtual void Sort() {
            Entries?.Sort();
        }

        public virtual void Refresh() {
            var entries = OptionGenerator?.Invoke();
            SetEntries(entries);
        }

        public virtual void SetEntries(List<ListEntry> entries) {
            Entries = entries;
            Sort();
        }

        private static void InitStyles() {
            if (_unselectedStyle == null) {
                _unselectedStyle = new GUIStyle(EditorStyles.label);
                _unselectedStyle.active = _unselectedStyle.normal;
            }

            if (_selectedStyle == null) {
                _selectedStyle = new GUIStyle(EditorStyles.label);
                _selectedStyle.normal = _selectedStyle.focused;
                _selectedStyle.active = _selectedStyle.focused;
            }
        }

        protected virtual bool DrawEntry(ListEntry entry) {

            if (!entry.Enabled) {
                return false;
            }

            using (new EditorGUILayout.HorizontalScope()) {
                EntriesShown++;

                if (!GUILayout.Button(entry.ToString(), GetEntryStyle(entry))) {
                    return false;
                }

                var time = EditorApplication.timeSinceStartup;
                var diff = time - _lastClickTime;
                _lastClickTime = EditorApplication.timeSinceStartup;

                return !NeedsDoubleClick || diff < 1;
            }
        }

        protected virtual void DrawEntries(List<ListEntry> entries) {

            if (entries == null) {
                return;
            }

            foreach (var entry in entries) {
                if (DrawEntry(entry)) {
                    _selected = entry;
                    SelectionHandler?.Invoke(entry);
                }
            }
        }

        protected virtual void DrawScrollView() {
            using (var scope = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUI.skin.box, GUILayout.ExpandHeight(true))) {
                _scrollPosition = scope.scrollPosition;
                DrawEntries(Entries);
            }
        }

        protected virtual void DrawFooter() {
            var message = EntriesShown == 0 ? "No assets shown." : $"{EntriesShown} assets shown.";
            GUILayout.Box(message, GUILayout.ExpandWidth(true));
        }

        protected virtual void DrawContent() {
            EntriesShown = 0;
            DrawScrollView();
            DrawFooter();
        }

        public virtual void OnGUI() {
            InitStyles();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));

            DrawContent();

            EditorGUILayout.EndVertical();
        }

    }

}