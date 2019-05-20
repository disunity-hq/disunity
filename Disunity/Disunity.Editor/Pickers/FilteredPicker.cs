using System;
using System.Collections.Generic;
using Disunity.Core;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Pickers {

    public class FilterSet : List<Action<List<IEntry>>> { }

    public class FilteredPicker : BasePicker {
        
        public FilterSet Filters;
        public bool ShowFilter { get; set; }
        public string Filter { get; protected set; }

        protected void DrawFilter() {
            using (new EditorGUILayout.HorizontalScope()) {
                GUILayout.Label("Search:", GUILayout.Width(70), GUILayout.ExpandWidth(false));
                Filter = EditorGUILayout.TextField(Filter ?? "");
            }
        }

        protected virtual void ApplyFilters() {
            foreach (var filter in Filters) {
                filter(_entries);
            }
        }

        protected override void DrawContent() {
            DrawFilter();
            base.DrawContent();
        }

        public virtual void SearchFilter(List<IEntry> entries) {
            foreach (var entry in _entries) {
                entry.SetEnabled(StringUtils.MatchesFilter(entry.AsString, Filter));
            }
        }

        protected virtual void InitializeFilters() {
            if (Filter == null) {
                Filter = "";
            }
            if (Filters == null) {
                Filters = new FilterSet() { SearchFilter };
            }
        }

        protected void OnGUI() {
            InitializeFilters();
            ApplyFilters();
            base.OnGUI();
        }

    }
}
