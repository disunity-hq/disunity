using System.Collections.Generic;
using System.Linq;

using Disunity.Editor.Components;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;

using UnityEditor;


namespace Disunity.Editor.Editors {

    internal abstract class BaseAssetEditor : BaseSelectionEditor {

        protected BaseAssetEditor(ExporterWindow window, FilteredPicker picker = null, Lister lister = null) : base(window, picker, lister) { }
        protected BaseAssetEditor(ExporterWindow window) : base(window, null, null) { }

        public virtual string GetAssetFilter() {
            return null;
        }

        public virtual string[] GetAssetPaths() {
            var assetFilter = GetAssetFilter();

            if (assetFilter != null) {
                return AssetDatabase
                       .FindAssets(assetFilter)
                       .Select(AssetDatabase.GUIDToAssetPath)
                       .ToArray();
            }

            return AssetDatabase.GetAllAssetPaths();
        }

        public AssetEntry AddTo(AssetEntry parent, string assetPath, string pathPart) {

            if (parent.Children == null) {
                parent.Children = new List<TreeEntry>();
            }

            foreach (AssetEntry child in parent.Children) {
                if (child.PathPart == pathPart) {
                    return child;
                }
            }

            var newEntry = new AssetEntry() {PathPart = pathPart, Value = assetPath, Enabled = true};
            parent.Add(newEntry);

            return newEntry;
        }

        public List<AssetEntry> GenerateGraph(string[] assets) {
            var map = new Dictionary<string, AssetEntry>();

            foreach (var asset in assets) {
                var parts = asset.Split('/').ToList();

                var firstPart = parts[1];
                parts.RemoveAt(0);
                parts.RemoveAt(0);

                var parentExists = map.ContainsKey(firstPart);
                var value = parts.Count > 1 ? null : asset;
                var parent = parentExists ? map[firstPart] : new AssetEntry() {PathPart = firstPart, Enabled = true};
                map[firstPart] = parent;

                foreach (var part in parts) {
                    parent = AddTo(parent, asset, part);
                }

                parent.Value = asset;
            }

            return map.Values.ToList();
        }

        public override List<ListEntry> GenerateOptions() {
            var paths = GetAssetPaths().Where(o => o.StartsWith("Assets/")).ToArray();
            var graph = GenerateGraph(paths);
            return new List<ListEntry>(graph);
        }

        public class AssetEntry : TreeEntry {

            public string PathPart { get; set; }

            public override string ToString() {
                return PathPart;
            }

        }

    }

}