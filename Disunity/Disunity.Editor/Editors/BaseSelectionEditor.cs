using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Components;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEditor;


namespace Disunity.Editor.Editors {
    abstract class BaseSelectionEditor : BaseEditor {
        protected readonly FilteredPicker _picker;
        protected readonly Lister _lister;

        public abstract List<ListEntry> GenerateOptions();
        public abstract string[] GetSelections();
        public abstract void SelectionRemoved(string selection);
        public abstract void SelectionAdded(ListEntry selection);

        protected BaseSelectionEditor(ExporterWindow window, FilteredPicker picker = null, Lister lister = null) : base(window) {
            _picker = picker ?? DefaultPicker();
            _lister = lister ?? DefaultLister();

            _lister.OnRemoved += SelectionRemoved;
            _picker.OptionGenerator = GenerateOptions;
            _picker.SelectionHandler = SelectionAdded;
            _picker.Filters.Add(SelectionFilter);
        }
        public virtual FilteredPicker DefaultPicker() {
            return new HierarchyPicker();
        }

        public virtual Lister DefaultLister() {
            return new Lister();
        }

        protected void SelectionFilter(List<ListEntry> entries) {
            var selections = GetSelections();

            foreach (var entry in entries) {
                var value = entry.Value;

                if (selections.Contains(value)) {
                    entry.Enabled = false;
                }

                if (entry is TreeEntry hierarchyEntry && hierarchyEntry.Children != null) {
                    SelectionFilter(new List<ListEntry>(hierarchyEntry.Children));
                }
            }
        }

        public override void Draw() {
            using (new EditorGUILayout.VerticalScope()) {

                var selections = GetSelections();
                if (selections.Length > 0)
                    _lister.OnGUI(selections);

                _picker.OnGUI();
            }
        }

        public override void Init() {
            _picker.Refresh();
        }

    }
}
