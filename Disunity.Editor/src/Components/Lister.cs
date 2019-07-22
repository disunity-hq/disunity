using System;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Components {

    public class Lister {

        public event Action<string> OnRemoved;

        protected virtual void DrawItem(string item) {
            EditorGUILayout.LabelField(item.ToString(), GUILayout.ExpandWidth(true));
        }

        protected bool DrawRemoveButton() {
            return GUILayout.Button("x", GUILayout.Width(24), GUILayout.Height(14));
        }

        protected void DrawItemRow(string item) {
            using (new EditorGUILayout.HorizontalScope()) {
                DrawItem(item);

                if (DrawRemoveButton()) {
                    OnRemoved?.Invoke(item);
                }
            }
        }

        public void DrawItems(string[] items) {
            foreach (var item in items) {
                DrawItemRow(item);
            }
        }

        public void OnGUI(string[] items) {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true))) {
                DrawItems(items);
            }
        }
    }

}