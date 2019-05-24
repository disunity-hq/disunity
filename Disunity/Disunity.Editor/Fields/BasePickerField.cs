using System.Collections.Generic;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Fields {

    public abstract class BasePickerField {

        public ExporterWindow Window { get; protected set; }
        public GUIStyle Style { get; set; }
        public BasePicker Picker { get; protected set; }
        public BasePickerWindow PickerWindow { get; protected set; }

        protected abstract List<ListEntry> GenerateOptions();
        public abstract string CurrentSelection();
        public abstract void SelectionMade(ListEntry obj);

        protected BasePickerField(ExporterWindow window, BasePicker picker = null, GUIStyle style = null) {
            Window = window;
            Picker = picker ?? DefaultPicker();

            Picker.OptionGenerator = GenerateOptions;
            Picker.SelectionHandler += SelectionMade;
            Picker.SelectionHandler += entry => window.Repaint();
        }

        public virtual BasePicker DefaultPicker() {
            return new BasePicker();
        }

        public virtual GUIStyle DefaultStyle() {
            return new GUIStyle("TextField") { fixedHeight = 16 };
        }

        public virtual string GetLabel() {
            return CurrentSelection() ?? "None selected.";
        }

        public virtual bool GetButton() {
            var label = GetLabel();
            return GUILayout.Button(label, Style);
        }

        public virtual void HandlePress() {
            PickerWindow = EditorWindow.GetWindow<BasePickerWindow>();
            PickerWindow.Picker = Picker;
            Picker.Refresh();
        }

        public void OnGUI() {
            Style = Style ?? DefaultStyle();
            if (GetButton()) {
                HandlePress();
            }
        }
    }
}