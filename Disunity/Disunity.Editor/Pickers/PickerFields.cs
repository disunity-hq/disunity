using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Disunity.Editor {
    class PickerFields {
        public static void AssetField<T>([CanBeNull] Object currentValue, Action<T> callback, Func<AssetPicker.HierarchyEntry, bool> filter = null) where T : Object{

            var guiContent = EditorGUIUtility.ObjectContent(currentValue, typeof(T));

            var style = new GUIStyle("TextField") {
                fixedHeight = 16,
                imagePosition = currentValue == null ? ImagePosition.ImageLeft : ImagePosition.TextOnly
            };


            if (GUILayout.Button(guiContent, style))
                AssetPicker.Show(typeof(T), null, o => callback(o as T), currentValue, filter);
        }
    }
}
