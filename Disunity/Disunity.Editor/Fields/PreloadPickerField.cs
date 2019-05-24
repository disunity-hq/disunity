using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEngine;


namespace Disunity.Editor.Fields {

    public class PreloadPickerField : ClassPickerField {

        public PreloadPickerField(ExporterWindow window, BasePicker picker = null, GUIStyle style = null) : base(window, picker, style) {  }

        public override string CurrentSelection() {
            return Window.Settings.PreloadAssembly;
        }

        public override void SelectionMade(ListEntry entry) {
            if (entry?.Value == null) {
                Window.Settings.PreloadAssembly = null;
                Window.Settings.PreloadClass = null;
            } else {
                var parts = entry.Value.Split('.');
                Window.Settings.PreloadAssembly = parts[0];
                Window.Settings.PreloadClass = parts[1];
            }
        }

        protected override string[] CandidateAssemblyPaths() {
            return Window.Settings.PreloadAssemblies;
        }
    }
}