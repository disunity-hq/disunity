using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Disunity.Shared;
using UnityEngine;


namespace Disunity.Core {

    internal class AssetBundleResource : Resource {

        private bool _canLoad;

        public AssetBundleResource(string name, string path) : base(name) {
            Path = path;

            _canLoad = false;

            GetAssetPaths();
        }

        public string Path { get; }

        public AssetBundle AssetBundle { get; private set; }

        public ReadOnlyCollection<string> AssetPaths { get; private set; }

        public override bool CanLoad => _canLoad;

        protected override IEnumerator LoadResources() {
            AssetBundle = AssetBundle.LoadFromFile(Path);

            yield break;
        }

        protected override IEnumerator LoadResourcesAsync() {
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path);

            while (!assetBundleCreateRequest.isDone) {
                LoadProgress = assetBundleCreateRequest.progress;
                yield return null;
            }

            AssetBundle = assetBundleCreateRequest.assetBundle;
        }

        protected override void UnloadResources() {
            if (AssetBundle != null) {
                AssetBundle.Unload(true);
            }

            AssetBundle = null;
        }

        private void GetAssetPaths() {
            var assetPaths = new List<string>();

            AssetPaths = assetPaths.AsReadOnly();

            if (string.IsNullOrEmpty(Path)) {
                return;
            }

            if (!File.Exists(Path)) {
                return;
            }

            var manifestPath = Path + ".manifest";

            if (!File.Exists(manifestPath)) {
                LogUtility.LogWarning(Name + " manifest missing");
                return;
            }

            _canLoad = true;

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
        }

    }

}