using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Disunity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Disunity.Runtime {
    public class RuntimeMod : Mod { 

        private readonly AssetBundleResource _assetsResource;
        private readonly AssetBundleResource _scenesResource;
        private readonly List<string> _runtimeAssemblyFiles = new List<string>();
        private readonly List<string> _runtimeAssemblyNames = new List<string>();
        private readonly List<Assembly> _runtimeAssemblies = new List<Assembly>();
        private readonly List<GameObject> _prefabs = new List<GameObject>();
        private readonly List<ModScene> _scenes = new List<ModScene>();

        public event EventHandler OnStart;
        public event EventHandler OnUpdate;

        public RuntimeMod(string infoPath) : base(infoPath) {

            Prefabs = _prefabs.AsReadOnly();
            Scenes = _scenes.AsReadOnly();

            var assets = System.IO.Path.Combine(InstallPath, "assetbundles", Info.Name.ToLower() + ".assets");
            var scenes = System.IO.Path.Combine(InstallPath, "assetbundles", Info.Name.ToLower() + ".scenes");

            _assetsResource = new AssetBundleResource(Info.Name + " assets", assets);
            _scenesResource = new AssetBundleResource(Info.Name + " scenes", scenes);

            AssetPaths = _assetsResource.AssetPaths;
            SceneNames = _scenesResource.AssetPaths;
            RuntimeAssemblyNames = _runtimeAssemblyNames.AsReadOnly();

            if (!CheckResources()) {
                return;
            }

            try {
                LoadAssemblies();
                LoadPrefabs();
                LoadScenes();
            } catch (Exception e) {
                Debug.LogException(e);
                SetInvalid();
            }
        }

        /// <summary>
        ///     Collection of names of RuntimeAssemblies included in this Mod.
        /// </summary>
        public ReadOnlyCollection<string> RuntimeAssemblyNames { get; private set; }

        /// <summary>
        ///     Collection of names of Scenes included in this Mod.
        /// </summary>
        public ReadOnlyCollection<string> SceneNames { get; private set; }

        /// <summary>
        ///     Collection of paths of assets included in this Mod.
        /// </summary>
        public ReadOnlyCollection<string> AssetPaths { get; private set; }

        /// <summary>
        ///     Collection of ModScenes included in this Mod.
        /// </summary>
        public ReadOnlyCollection<ModScene> Scenes { get; private set; }

        /// <summary>
        ///     Collection of loaded prefabs included in this Mod. Only available when the mod is loaded.
        /// </summary>
        public ReadOnlyCollection<GameObject> Prefabs { get; private set; }

        public void InvokeOnStart() {
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        public void InvokeOnUpdate() {
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }

        private bool CheckResources() {
            Debug.Log("Checking Resources...");

            var contentTypes = (ContentType) Info.ContentTypes;

            if (contentTypes.HasFlag(ContentType.Prefabs) && !_assetsResource.IsValid) {
                IsValid = false;
                Debug.Log("Assets assetbundle missing for Mod: " + Info.Name);
            }

            if (contentTypes.HasFlag(ContentType.Scenes) && !_scenesResource.IsValid) {
                IsValid = false;
                Debug.Log("Scenes assetbundle missing for Mod: " + Info.Name);
            }

            if (contentTypes.HasFlag(ContentType.RuntimeAssemblies) && Info.RuntimeAssemblies.Length == 0) {
                IsValid = false;
                Debug.Log("Runtime assembly metadata missing for Mod: " + Info.Name);
            }

            foreach (var path in Info.RuntimeAssemblies) {
                var modPath = Path.GetDirectoryName(Info.Path);
                var fullPath = Path.Combine(modPath, path);

                if (File.Exists(fullPath)) {
                    _runtimeAssemblyFiles.Add(fullPath);
                    _runtimeAssemblyNames.Add(System.IO.Path.GetFileName(fullPath));
                    continue;
                }

                IsValid = false;
                Debug.Log(path + " missing for Mod: " + Info.Name);
            }

            return IsValid;

        }

        protected void LoadScenes() {
            foreach (var sceneName in SceneNames) {
                var modScene = new ModScene(sceneName, this);
                _scenes.Add(modScene);
            }
        }

        protected void LoadPrefabs() {
            _prefabs.AddRange(_assetsResource.AssetBundle.LoadAllAssets<GameObject>());
        }

        private void LoadAssemblies() {
            foreach (var path in _runtimeAssemblyFiles) {
                if (!File.Exists(path)) {
                    continue;
                }

                try {
                    var assembly = Assembly.Load(File.ReadAllBytes(path));
                    assembly.GetTypes();
                    _runtimeAssemblies.Add(assembly);
                } catch (Exception e) {
                    Debug.LogError(e);
                    SetInvalid();
                }
            }
        }

        /// <summary>
        ///     Get an asset with name.
        /// </summary>
        /// <param name="name">The asset's name.</param>
        /// <returns>The asset if it has been found. Null otherwise</returns>
        public Object GetAsset(string name) {
            if (_assetsResource.IsValid) {
                return _assetsResource.AssetBundle.LoadAsset(name);
            }

            return null;
        }

        /// <summary>
        ///     Get an asset with name of a certain Type.
        /// </summary>
        /// <param name="name">The asset's name.</param>
        /// <typeparam name="T">The asset Type.</typeparam>
        /// <returns>The asset if it has been found. Null otherwise</returns>
        public T GetAsset<T>(string name) where T : Object {
            if (_assetsResource.IsValid) {
                return _assetsResource.AssetBundle.LoadAsset<T>(name);
            }

            return null;
        }

        /// <summary>
        ///     Get all assets of a certain Type.
        /// </summary>
        /// <typeparam name="T">The asset Type.</typeparam>
        /// <returns>AssetBundleRequest that can be used to get the asset.</returns>
        public T[] GetAssets<T>() where T : UnityEngine.Object {
            if (_assetsResource.IsValid) {
                return _assetsResource.AssetBundle.LoadAllAssets<T>();
            }

            return new T[0];
        }

        /// <summary>
        ///     Get an asset with name of a certain Type.
        /// </summary>
        /// <param name="name">The asset's name.</param>
        /// <typeparam name="T">The asset's Type</typeparam>
        /// <returns>AssetBundleRequest that can be used to get the asset.</returns>
        public AssetBundleRequest GetAssetAsync<T>(string name) where T : UnityEngine.Object {
            if (_assetsResource.IsValid) {
                return _assetsResource.AssetBundle.LoadAssetAsync<T>(name);
            }

            return null;
        }

        /// <summary>
        ///     Get all assets of a certain Type.
        /// </summary>
        /// <typeparam name="T">The asset Type.</typeparam>
        /// <returns>AssetBundleRequest that can be used to get the assets.</returns>
        public AssetBundleRequest GetAssetsAsync<T>() where T : UnityEngine.Object {
            if (_assetsResource.IsValid) {
                return _assetsResource.AssetBundle.LoadAllAssetsAsync<T>();
            }

            return null;
        }

        /// <summary>
        ///     Get all Components of type T in all prefabs
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T[] GetComponentsInPrefabs<T>() {
            var components = new List<T>();

            foreach (var prefab in Prefabs) {
                components.AddRange(prefab.GetComponentsInChildren<T>());
            }

            return components.ToArray();
        }

        /// <summary>
        ///     Get all Components of type T in all loaded ModScenes.
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T[] GetComponentsInScenes<T>() {
            if (!typeof(T).IsSubclassOf(typeof(Component))) {
                throw new ArgumentException(typeof(T).Name + " is not a component.");
            }

            var components = new List<T>();

            foreach (var scene in _scenes) {
                components.AddRange(scene.GetComponentsInScene<T>());
            }

            return components.ToArray();
        }

        /// <summary>
        ///     Get instances of all Types included in the Mod that implement or derive from Type T.
        ///     Reuses existing instances and creates new instances for Types that have no instance yet.
        ///     Does not instantiate Components; returns all active instances of the Component instead.
        /// </summary>
        /// <typeparam name="T">The Type that will be looked for</typeparam>
        /// <param name="args">Optional arguments for the Type's constructor</param>
        /// <returns>A List of Instances of Types that implement or derive from Type T</returns>
        public T[] GetInstances<T>(params object[] args) {
            var instances = new List<T>();

            if (IsValid == false) {
                return instances.ToArray();
            }

            foreach (var assembly in _runtimeAssemblies) {
                try {
                    instances.AddRange(GetInstances<T>(assembly, args));
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }

            return instances.ToArray();
        }

        private T[] GetInstances<T>(Assembly assembly, params object[] args) {
            var instances = new List<T>();

            foreach (var type in assembly.GetTypes()) {
                if (!typeof(T).IsAssignableFrom(type)) {
                    continue;
                }

                if (type.IsAbstract) {
                    continue;
                }

                if (!type.IsClass) {
                    continue;
                }

                if (type.IsSubclassOf(typeof(Component))) {
                    foreach (var component in GetComponents(type)) {
                        instances.Add((T)(object)component);
                    }

                    continue;
                }

                try {
                    var instance = (T)Activator.CreateInstance(type, args);
                    instances.Add(instance);
                } catch (Exception e) {
                    if (e is MissingMethodException) {
                        Debug.Log(e.Message);
                    } else {
                        Debug.LogException(e);
                    }
                }
            }

            return instances.ToArray();
        }

        private static Component[] GetComponents(Type componentType) {
            var components = new List<Component>();

            for (var i = 0; i < SceneManager.sceneCount; i++) {
                components.AddRange(SceneManager.GetSceneAt(i).GetComponentsInScene(componentType));
            }

            return components.ToArray();
        }

        //public override void UpdateConflicts(Mod other) {
        //    Debug.Log("[Disunity] Skipping collision check.");
        //}
    }

}
