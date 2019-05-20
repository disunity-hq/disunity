using System;
using System.Collections.Generic;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Fields {

    public class BasePickerField<TS, TP> where TS : class, IEntry where TP : BasePicker {

        public TS Selection { get; set; }
        public TP Picker { get; protected set; }
        public GUIStyle Style { get; set; }
        public FilterSet Filters { get; protected set; }

        public BasePickerField(GUIStyle style = null, FilterSet filters = null) {
            Style = style;
            Filters = filters;
        }

        public virtual GUIStyle DefaultStyle() {
            return new GUIStyle("TextField") { fixedHeight = 16 };
        }

        public virtual string GetSelectionLabel() {
            return Selection.AsString;
        }

        public virtual string GetLabel() {
            return Selection == null ? "None selected." : GetSelectionLabel();
        }

        public virtual bool GetButton() {
            var label = GetLabel();
            return GUILayout.Button(label, Style);
        }

        public virtual void HandlePress(Func<List<IEntry>> generator, Action<BasePickerField<TS, TP>> handler) {
            var entries = generator();
            Picker = EditorWindow.GetWindow<TP>();
            Picker.Set(entries);
            Picker.OnSelection += (o, s) => {
                Selection = (TS) s;
                handler(this);
            };
        }

        public void OnGUI(Func<List<IEntry>> generator, Action<BasePickerField<TS, TP>> handler, GUIStyle style = null) {
            Style = style ?? (Style ?? DefaultStyle());
            if (GetButton()) {
                HandlePress(generator, handler);
            }
        }

    }

}