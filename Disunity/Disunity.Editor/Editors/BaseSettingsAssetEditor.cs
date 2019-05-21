using Disunity.Editor.Pickers;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Disunity.Editor.Editors {
    internal abstract class BaseSettingsAssetEditor<T> : BaseAssetEditor where T : Object {

        protected readonly ExportSettings _settings;

        protected abstract IEnumerable<T> Setting { get; set; }

        public BaseSettingsAssetEditor(EditorWindow window, ExportSettings settings) : base(window) {
            _settings = settings;
        }

        protected override void HandleAddition(AssetEntry addition) {
            base.HandleAddition(addition);
            if (Setting.All(asmDef => AssetDatabase.GetAssetPath(asmDef) != addition.Value)) {
                Setting = 
                    Enumerable.Concat(
                        Setting,
                        Enumerable.Repeat(AssetDatabase.LoadAssetAtPath<T>(addition.Value), 1))
                    .ToArray();
            }
        }

        protected override void HandleSubtraction(AssetEntry subtraction) {
            base.HandleSubtraction(subtraction);
            Setting = Setting
                .Where(asmDef => AssetDatabase.GetAssetPath(asmDef) != subtraction.Value)
                .ToArray();
        }
        public override string[] GetAssetPaths() {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
        }
    }
}
