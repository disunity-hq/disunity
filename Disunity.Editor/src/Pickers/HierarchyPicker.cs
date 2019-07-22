using System.Collections.Generic;
using System.Linq;

using Disunity.Core;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor.Pickers {

    public class TreeEntry : ListEntry {

        public TreeEntry Parent { get; set; }
        public List<TreeEntry> Children { get; set; }
        public bool Expanded { get; set; }

        public void SetEnabledRecursive(bool enabled) {
            Enabled = enabled;

            if (Children != null) {
                foreach (var child in Children) {
                    child.SetEnabledRecursive(enabled);
                }

                Sort();
            }
        }

        public void Add(TreeEntry child) {
            if (Children == null) {
                Children = new List<TreeEntry>();
            }

            child.Parent = this;
            Children.Add(child);
            Sort();
        }

        public void Sort() {
            if (Children == null) {
                return;
            }

            Children.Sort();

            foreach (var child in Children) {
                child.Sort();
            }
        }

    }

    internal class HierarchyPicker : FilteredPicker {

        public override void Sort() {
            Entries.Sort();

            foreach (TreeEntry entry in Entries) {
                entry.Sort();
            }
        }

        protected void DrawParent(TreeEntry listEntry) {

            if (listEntry.Children == null || !listEntry.Children.Any(o => o.Enabled)) {
                return;
            }

            listEntry.Expanded = EditorGUILayout.Foldout(listEntry.Expanded, listEntry.ToString(), true);

            if (listEntry.Expanded) {
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.Space(10);

                    using (new EditorGUILayout.VerticalScope()) {
                        DrawEntries(new List<ListEntry>(listEntry.Children));
                    }
                }
            }
        }

        protected override bool DrawEntry(ListEntry entry) {
            var treeEntry = (TreeEntry) entry;

            if (treeEntry.Children == null) {
                return base.DrawEntry(entry);
            }

            DrawParent(treeEntry);
            return false;
        }

        public override void SearchFilter(List<ListEntry> entries) {
            foreach (TreeEntry entry in entries) {
                entry.SetEnabledRecursive(Filter.Length <= 2 || StringUtils.MatchesFilter(entry.ToString(), Filter));

                if (entry.Enabled) {
                    var parent = entry.Parent;

                    while (parent != null) {
                        parent.Enabled = true;
                        parent = parent.Parent;
                    }

                    continue;
                }

                if (entry.Enabled == false && entry.Children != null) {
                    var children = new List<ListEntry>(entry.Children);
                    SearchFilter(children);
                }
            }
        }

    }

}