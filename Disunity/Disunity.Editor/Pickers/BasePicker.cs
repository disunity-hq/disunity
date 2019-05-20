using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Pickers {

    public interface IEntry : IComparable {
        string AsString { get; }
        bool Enabled { get; }
        void SetEnabled(bool enabled);
    }

    public class GenericEntry : IEntry {
        public IComparable value;
        public virtual string AsString => value.ToString();
        public bool Enabled { get; protected set; }
        public void SetEnabled(bool enabled) => Enabled = enabled;
        public int CompareTo(object obj) {
            if (obj is GenericEntry val)
                return value.CompareTo(val.value);
            return -1;
        }
    }

    public class BasePicker : EditorWindow {

        public event EventHandler<IEntry> OnSelection;

        protected IEntry _selected;
        protected static GUIStyle _commonStyle;
        protected static GUIStyle _selectedStyle;
        protected List<IEntry> _entries = new List<IEntry>();
        protected Vector2 _scroll;
        protected double _lastClick;
        public bool NeedsDoubleClick { get; set; }
        public int EntriesShown { get; protected set; }

        protected virtual GUIStyle GetEntryStyle(IEntry entry) {
            return entry == _selected ? _selectedStyle : _commonStyle;
        }

        protected virtual void HandleSelection(IEntry entry) {
            _selected = entry;
            OnSelection?.Invoke(this, entry);
        }

        public virtual void Add(IEntry entry, bool repaint = false) {
            _entries.Add(entry);
            if (repaint) {
                Sort();
                Repaint();
            }
        }

        public virtual void Set(List<IEntry> entries, bool repaint = false) {
            _entries = entries;
            if (repaint) {
                Sort();
                Repaint();
            }
        }

        protected virtual void DrawEntry(IEntry entry) {
            using (new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button(entry.AsString, GetEntryStyle(entry))) {
                    var time = EditorApplication.timeSinceStartup;
                    var diff = time - _lastClick;
                    if (!NeedsDoubleClick || diff < 1) {
                        HandleSelection(entry);
                    }
                    _lastClick = EditorApplication.timeSinceStartup;
                }
            }
        }

        protected virtual void DrawEntries(List<IEntry> entries) {
            EntriesShown = 0;

            if (entries == null) {
                return;
            }

            foreach (var entry in entries) {
                if (entry.Enabled) {
                    DrawEntry(entry);
                    EntriesShown++;
                }
                Debug.Log($"Time: {Time.time}");
            }
        }

        protected virtual void DrawScrollView() {
            using (var scope = new EditorGUILayout.ScrollViewScope(_scroll, GUI.skin.box, GUILayout.ExpandHeight(true))) {
                _scroll = scope.scrollPosition;
                DrawEntries(_entries);
            }
        }

        protected virtual void DrawFooter() {
            GUILayout.Box(EntriesShown + " assets shown.", GUILayout.ExpandWidth(true));
        }

        protected virtual void DrawContent() {
            DrawScrollView();
            DrawFooter();
        }

        private static void InitStyles() {
            if (_commonStyle == null) {
                _commonStyle = new GUIStyle(EditorStyles.label);
                _commonStyle.active = _commonStyle.normal;
            }

            if (_selectedStyle == null) {
                _selectedStyle = new GUIStyle(EditorStyles.label);
                _selectedStyle.normal = _selectedStyle.focused;
                _selectedStyle.active = _selectedStyle.focused;
            }
        }

        public virtual void Sort() {
            _entries.Sort();
        }

        protected void OnGUI() {
            InitStyles();
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(false))) {

                DrawContent();
            }
        }

        public void SetEnabled(IEntry entry, bool enabled) {
            if (entry.Enabled != enabled) {
                entry.SetEnabled(enabled);
                Sort();
                Repaint();
            }
        }
    }
}
