using System;
using System.Collections.Generic;
using Disunity.Editor.Pickers;
using UnityEditor;


namespace Disunity.Editor.Windows {

    public class BasePickerWindow : EditorWindow {

        public BasePicker Picker;

        public virtual void Set(List<ListEntry> entries) { 
            Picker?.SetEntries(entries);
        }

        public virtual void Sort() {
            Picker?.Sort();
        }

        public virtual void Refresh() {
            Picker?.Refresh();
        }

        protected void OnGUI() {
            Picker.OnGUI();
        }
    }
}
