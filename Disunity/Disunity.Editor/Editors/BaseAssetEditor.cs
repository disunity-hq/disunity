using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor.Editors {
    abstract class BaseAssetEditor : BaseSelectionEditor<AssetEntry, AssetPicker> {

        public BaseAssetEditor(EditorWindow window) : base(window) { }

        public virtual string[] GetAssetPaths() {
            return AssetDatabase.GetAllAssetPaths();
        }

        public override List<IEntry> Generator() {
            return new List<IEntry>(GenerateGraph(GetAssetPaths()));
        }

        public static AssetEntry AddTo(AssetEntry parent, string assetPath, string pathPart) {

            if (parent.Children == null) {
                parent.Children = new List<ITreeEntry>();
            }

            foreach (AssetEntry child in parent.Children) {
                if (child.PathPart == pathPart) return child;
            }

            var newEntry = new AssetEntry() { PathPart = pathPart, value = assetPath };
            parent.Add(newEntry);

            return newEntry;
        }

        public static List<AssetEntry> GenerateGraph(string[] assets) {

            Debug.Log($"Assets targetted: {assets.Length}");
            var map = new Dictionary<string, AssetEntry>();

            foreach (var asset in assets) {
                var parts = asset.Split('/').ToList();

                var firstPart = parts[0];
                parts.RemoveAt(0);

                var parentExists = map.ContainsKey(firstPart);
                var parent = parentExists ? map[firstPart] : new AssetEntry() { PathPart = firstPart, value = asset };
                map[firstPart] = parent;

                foreach (var part in parts) {
                    parent = AddTo(parent, asset, part);
                }
            }

            return map.Values.ToList();
        }
    }
}
