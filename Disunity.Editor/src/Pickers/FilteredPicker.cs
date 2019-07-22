using System;
using System.Collections.Generic;

using Disunity.Core;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor.Pickers {

    public class FilterSet : List<Action<List<ListEntry>>> { }

    public class FilteredPicker : BasePicker {

        public FilterSet Filters = new FilterSet();

        public FilteredPicker() {
            InitializeFilters();
        }

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
                filter(Entries);
            }
        }

        protected override void DrawContent() {
            DrawFilter();
            base.DrawContent();
        }

        public virtual void SearchFilter(List<ListEntry> entries) {
            if (Filter == null) {
                return;
            }

            foreach (var entry in Entries) {
                entry.Enabled = StringUtils.MatchesFilter(entry.ToString(), Filter);
            }
        }

        protected virtual void InitializeFilters() {
            if (Filter == null) {
                Filter = "";
            }

            if (!Filters.Contains(SearchFilter)) {
                Filters.Insert(0, SearchFilter);
            }
        }

        public override void OnGUI() {
            ApplyFilters();
            base.OnGUI();
        }

    }

}