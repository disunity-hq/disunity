using System.Collections.Generic;
using Disunity.Core;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Pickers {

    public interface ITreeEntry : IEntry {
        ITreeEntry Parent { get; set; }
        List<ITreeEntry> Children { get; set; }
        bool Expanded { get; set; }
        void SetEnabledRecursive(bool enabled);
        void Sort();
    }

    public class HierarchyEntry : GenericEntry, ITreeEntry {
        public ITreeEntry Parent { get; set; }
        public List<ITreeEntry> Children { get; set; }
        public bool Expanded { get; set; }
        public void SetEnabledRecursive(bool enabled) {
            SetEnabled(enabled);
            if (Children != null) {
                foreach (var child in Children) {
                    child.SetEnabledRecursive(enabled);
                }

                Sort();
            }
        }

        public void Add(ITreeEntry child) {
            if (Children == null) {
                Children = new List<ITreeEntry>();
            }

            child.Parent = this;
            Children.Add(child);
            Sort();
        }

        public void Sort() {
            if (Children == null) return;
            Children.Sort();
            foreach (var child in Children) {
                child.Sort();
            }
        }
    }

    abstract class HierarchyPicker : FilteredPicker {

        public override void Sort() {
            _entries.Sort();
            foreach (ITreeEntry entry in _entries) {
                entry.Sort();
            }
        }

        protected void DrawParent(ITreeEntry entry) {
            entry.Expanded = EditorGUILayout.Foldout(entry.Expanded, entry.AsString, true);

            if (entry.Expanded) {
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.Space(10);
                    using (new EditorGUILayout.VerticalScope()) {
                        DrawEntries(new List<IEntry>(entry.Children));
                    }
                }
            }
        }

        protected override void DrawEntry(IEntry entry) {
            var treeEntry = (ITreeEntry)entry;

            if (treeEntry.Children == null) {
                base.DrawEntry(entry);
            }
            else {
                DrawParent(treeEntry);
            }
        }

        public override void SearchFilter(List<IEntry> entries) {
            foreach (ITreeEntry entry in entries) {
                entry.SetEnabledRecursive(Filter.Length <= 2 || StringUtils.MatchesFilter(entry.AsString, Filter));
                if (entry.Enabled) {
                    var parent = entry.Parent;
                    while (parent != null) {
                        parent.SetEnabled(true);
                        parent = parent.Parent;
                    }

                    continue;
                }

                if (entry.Enabled == false && entry.Children != null) {
                    var children = new List<IEntry>(entry.Children);
                    SearchFilter(children);
                }
            }
        }
    }
}
