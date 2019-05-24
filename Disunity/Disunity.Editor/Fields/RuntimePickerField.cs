using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEngine;


namespace Disunity.Editor.Fields {

    public class RuntimePickerField : ClassPickerField {

        public RuntimePickerField(ExporterWindow window, FilteredPicker picker = null, GUIStyle style = null) : base(window, picker, style) { }

        public override string CurrentSelection() {
            return Window.Settings.RuntimeAssembly;
        }

        public override void SelectionMade(ListEntry entry) {
            if (entry?.Value == null) {
                Window.Settings.RuntimeAssembly = null;
                Window.Settings.RuntimeClass = null;
            }
            else {
                var parts = entry.Value.Split('.');
                Window.Settings.RuntimeAssembly = parts[0];
                Window.Settings.RuntimeClass = parts[1];
            }
        }

        protected override string[] CandidateAssemblyPaths() {
            return Window.Settings.RuntimeAssemblies;
        }

    }

}