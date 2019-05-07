using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;


namespace Disunity.Runtime {

    internal class AssetBundleResource {

        public bool IsValid { get; private set; }

        public AssetBundleResource(string name, string path) {
            Path = path;
            IsValid = false;

            if (GetAssetPaths()) {
                AssetBundle = AssetBundle.LoadFromFile(Path);
            }
        }

        public string Path { get; }

        [CanBeNull] public AssetBundle AssetBundle { get; private set; }

        public ReadOnlyCollection<string> AssetPaths { get; private set; }

        private bool GetAssetPaths() {
            var assetPaths = new List<string>();

            AssetPaths = assetPaths.AsReadOnly();

            if (string.IsNullOrEmpty(Path)) {
                IsValid = false;
                return false;
            }

            if (!File.Exists(Path)) {
                IsValid = false;
                return false;
            }

            var manifestPath = Path + ".manifest";

            IsValid = true;

            //TODO: long lines in manifest are formatted?
            var lines = File.ReadAllLines(manifestPath);

            var start = Array.IndexOf(lines, "Assets:") + 1;

            for (var i = start; i < lines.Length; i++) {
                if (!lines[i].StartsWith("- ")) {
                    break;
                }

                var assetPath = lines[i].Substring(2);

                //Note: if the asset is a scene, we only need the name
                if (assetPath.EndsWith(".unity")) {
                    assetPath = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                }

                assetPaths.Add(assetPath);
            }

            return true;
        }

    }

}