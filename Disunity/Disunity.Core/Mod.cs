using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Disunity.Cecil;
using Disunity.Interface;
using Disunity.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


namespace Disunity.Core {

    /// <summary>
    ///     Class that represents a Mod.
    ///     A Mod lets you load scenes, prefabs and RuntimeAssemblies that have been exported.
    /// </summary>
    public class Mod : Resource {

        private readonly List<string> _runtimeAssemblyFiles;

        private readonly AssetBundleResource _assetsResource;
        private readonly AssetBundleResource _scenesResource;
        private List<string> _runtimeAssemblyNames;
        private List<Mod> _conflictingMods;
        private List<GameObject> _prefabs;
        private List<ModScene> _scenes;

        private Dictionary<Type, object> _allInstances;
        private List<Assembly> _runtimeAssemblies;

        /// <summary>
        ///     Initialize a new Mod with a path to a mod file.
        /// </summary>
        /// <param name="path">The path to a mod file</param>
        public Mod(string path) : base(Path.GetFileNameWithoutExtension(path)) {
            ModInfo = ModInfo.Load(path);

            ContentTypes = ModInfo.ContentTypes;

            var modDirectory = Path.GetDirectoryName(path);
            var platformDirectory = Path.Combine(modDirectory, Application.platform.GetModPlatform().ToString());

            var assets = Path.Combine(platformDirectory, ModInfo.Name.ToLower() + ".assets");
            var scenes = Path.Combine(platformDirectory, ModInfo.Name.ToLower() + ".scenes");

            _runtimeAssemblyFiles = AssemblyUtility.GetAssemblies(modDirectory, AssemblyFilter.ModAssemblies);
            _assetsResource = new AssetBundleResource(Name + " assets", assets);
            _scenesResource = new AssetBundleResource(Name + " scenes", scenes);

            IsValid = true;

            Initialize();
            CheckResources();
        }

        /// <summary>
        ///     Collection of names of RuntimeAssemblies included in this Mod.
        /// </summary>
        public ReadOnlyCollection<string> AssemblyNames { get; private set; }

        /// <summary>
        ///     Collection of Mods that are in conflict with this Mod.
        /// </summary>
        public ReadOnlyCollection<Mod> ConflictingMods { get; private set; }

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

        /// <summary>
        ///     This mod's ModInfo.
        /// </summary>
        public ModInfo ModInfo { get; }

        /// <summary>
        ///     Types of content included in this Mod.
        /// </summary>
        public ContentType ContentTypes { get; }

        /// <summary>
        ///     Is this Mod or any of its resources currently busy loading?
        /// </summary>
        public override bool IsBusy {
            get { return base.IsBusy || _scenes.Any(s => s.IsBusy); }
        }

        /// <summary>
        ///     Can this mod be loaded? False if a conflicting mod is loaded, if the mod is not enabled or if the mod is not valid
        /// </summary>
        public override bool CanLoad {
            get {
                CheckResources();
                return !ConflictingModsLoaded() && IsValid;
            }
        }

        /// <summary>
        ///     Set the mod to be enabled or disabled
        /// </summary>
        public bool IsEnabled {
            get => ModInfo.IsEnabled;
            set {
                ModInfo.IsEnabled = value;
                ModInfo.Save();
            }
        }

        /// <summary>
        ///     Is the mod valid? A Mod becomes invalid when it is no longer being managed by the ModManager,
        ///     when any of its resources is missing or can't be loaded.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        ///     The Mod's ContentHandler. Use for instantiating Objects and adding Components that have to be initialized for this
        ///     mod,
        ///     or cleaned up after the mod is unloaded.
        /// </summary>
        public ContentHandler ContentHandler { get; set; }

        /// <summary>
        ///     Occurs when a ModScene has been loaded
        /// </summary>
        public event Action<ModScene> SceneLoaded;

        /// <summary>
        ///     Occurs when a ModScene has been unloaded
        /// </summary>
        public event Action<ModScene> SceneUnloaded;

        /// <summary>
        ///     Occurs when a ModScene has cancelled async loading.
        /// </summary>
        public event Action<ModScene> SceneLoadCancelled;

        private void Initialize() {
            _allInstances = new Dictionary<Type, object>();
            _runtimeAssemblies = new List<Assembly>();
            _prefabs = new List<GameObject>();
            _scenes = new List<ModScene>();
            _conflictingMods = new List<Mod>();
            _runtimeAssemblyNames = new List<string>();

            Prefabs = _prefabs.AsReadOnly();
            Scenes = _scenes.AsReadOnly();
            ConflictingMods = _conflictingMods.AsReadOnly();
            AssemblyNames = _runtimeAssemblyNames.AsReadOnly();

            AssetPaths = _assetsResource.AssetPaths;
            SceneNames = _scenesResource.AssetPaths;

            _assetsResource.Loaded += OnAssetsResourceLoaded;
            _scenesResource.Loaded += OnScenesResourceLoaded;

            foreach (var sceneName in SceneNames) {
                var modScene = new ModScene(sceneName, this);

                modScene.Loaded += OnSceneLoaded;
                modScene.Unloaded += OnSceneUnloaded;
                modScene.LoadCancelled += OnSceneLoadCancelled;

                _scenes.Add(modScene);
            }

            foreach (var assembly in _runtimeAssemblyFiles) {
                _runtimeAssemblyNames.Add(Path.GetFileName(assembly));
            }

            ContentHandler = new ContentHandler(this, _scenes.Cast<IResource>().ToList().AsReadOnly(), Prefabs);
        }

        private void CheckResources() {
            Debug.Log("Checking Resources...");
            if (!ModInfo.Platforms.HasRuntimePlatform(Application.platform)) {
                IsValid = false;
                Debug.Log("Platform not supported for Mod: " + Name);

                return;
            }

            if (ContentTypes.HasFlag(Shared.ContentType.Prefabs) && !_assetsResource.CanLoad) {
                IsValid = false;
                Debug.Log("Assets assetbundle missing for Mod: " + Name);
            }

            if (ContentTypes.HasFlag(Shared.ContentType.Scenes) && !_scenesResource.CanLoad) {
                IsValid = false;
                Debug.Log("Scenes assetbundle missing for Mod: " + Name);
            }

            if (ContentTypes.HasFlag(Shared.ContentType.RuntimeAssemblies) && _runtimeAssemblyFiles.Count == 0) {
                IsValid = false;
                Debug.Log("RuntimeAssemblies missing for Mod: " + Name);
            }

            foreach (var path in _runtimeAssemblyFiles) {
                if (!File.Exists(path)) {
                    IsValid = false;
                    Debug.Log(path + " missing for Mod: " + Name);
                }
            }
        }

        private void VerifyAssemblies() {
            //if (!AssemblyVerifier.VerifyAssemblies(assemblyFiles))
            //{
            //    SetInvalid();
            //    Debug.Log("Incompatible assemblies found for Mod: " + name);
            //}
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
                }
                catch (Exception e) {
                    LogUtility.LogException(e);
                    SetInvalid();
                    Unload();
                }
            }
        }

        private void OnAssetsResourceLoaded(Resource resource) {
            try {
                if (_assetsResource.AssetBundle == null) {
                    throw new Exception("Could not load assets.");
                }

                var prefabs = _assetsResource.AssetBundle.LoadAllAssets<GameObject>();
                _prefabs.AddRange(prefabs);
            }
            catch (Exception e) {
                LogUtility.LogException(e);
                SetInvalid();
                Unload();
            }
        }

        private void OnScenesResourceLoaded(Resource resource) {
            if (_scenesResource.AssetBundle == null) {
                LogUtility.LogError("Could not load scenes.");
                SetInvalid();
                Unload();
            }
        }

        protected override IEnumerator LoadResources() {
            LogUtility.LogInfo("Loading Mod: " + Name);

            LoadAssemblies();

            _assetsResource.Load();

            _scenesResource.Load();

            yield break;
        }

        protected override IEnumerator LoadResourcesAsync() {
            LogUtility.LogInfo("Async loading Mod: " + Name);

            LoadAssemblies();

            _assetsResource.LoadAsync();

            _scenesResource.LoadAsync();

            yield return UpdateProgress(_assetsResource, _scenesResource);
        }

        private IEnumerator UpdateProgress(params Resource[] resources) {
            if (resources == null || resources.Length == 0) {
                yield break;
            }

            var loadingResources = resources.Where(r => r.CanLoad);

            var count = loadingResources.Count();

            while (true) {
                var isDone = true;
                float progress = 0;

                foreach (var resource in loadingResources) {
                    isDone = isDone && resource.LoadState == ResourceLoadState.Loaded;
                    progress += resource.LoadProgress;
                }

                LoadProgress = progress / count;

                if (isDone) {
                    yield break;
                }

                yield return null;
            }
        }

        protected override void PreUnLoadResources() {
            ContentHandler.Clear();

            _scenes.ForEach(s => s.Unload());

            foreach (var loader in GetInstances<IModHandler>()) {
                loader.OnUnloaded();
            }
        }

        protected override void UnloadResources() {
            LogUtility.LogInfo("Unloading Mod: " + Name);

            _allInstances.Clear();
            _runtimeAssemblies.Clear();
            _prefabs.Clear();

            _assetsResource.Unload();
            _scenesResource.Unload();

            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        private void OnSceneLoaded(Resource scene) {
            SceneLoaded?.Invoke((ModScene) scene);
        }

        private void OnSceneLoadCancelled(Resource scene) {
            SceneLoadCancelled?.Invoke((ModScene) scene);

            if (!_scenes.Any(s => s.IsBusy)) {
                End();
            }
        }

        private void OnSceneUnloaded(Resource scene) {
            SceneUnloaded?.Invoke((ModScene) scene);

            if (!_scenes.Any(s => s.IsBusy)) {
                End();
            }
        }

        protected override void OnLoadResumed() {
            //resume scene loading
            foreach (var scene in _scenes) {
                if (scene.LoadState == ResourceLoadState.Cancelling) {
                    scene.Load();
                }
            }

            base.OnLoadResumed();
        }

        protected override void OnLoaded() {
            foreach (var loader in GetInstances<IModHandler>()) {
                loader.OnLoaded(ContentHandler);
            }

            base.OnLoaded();
        }

        /// <summary>
        ///     Update this Mod's conflicting Mods with the supplied Mod
        /// </summary>
        /// <param name="other">Another Mod</param>
        public void UpdateConflicts(Mod other) {
            if (other == this || !IsValid) {
                return;
            }

            if (!other.IsValid) {
                if (_conflictingMods.Contains(other)) {
                    _conflictingMods.Remove(other);
                }

                return;
            }

            foreach (var assemblyName in _runtimeAssemblyNames)
            foreach (var otherAssemblyName in other.AssemblyNames) {
                if (assemblyName == otherAssemblyName) {
                    Debug.Log("Assembly " + other.Name + "/" + otherAssemblyName + " conflicting with " + Name + "/" +
                              assemblyName);

                    if (!_conflictingMods.Contains(other)) {
                        _conflictingMods.Add(other);
                        return;
                    }
                }
            }

            foreach (var sceneName in SceneNames)
            foreach (var otherSceneName in other.SceneNames) {
                if (sceneName == otherSceneName) {
                    Debug.Log("Scene " + other.Name + "/" + otherSceneName + " conflicting with " + Name + "/" +
                              sceneName);

                    if (!_conflictingMods.Contains(other)) {
                        _conflictingMods.Add(other);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///     Update this Mod's conflicting Mods with the supplied Mods
        /// </summary>
        /// <param name="mods">A collection of Mods</param>
        public void UpdateConflicts(IEnumerable<Mod> mods) {
            foreach (var mod in mods) {
                UpdateConflicts(mod);
            }
        }

        /// <summary>
        ///     Is another conflicting Mod loaded?
        /// </summary>
        /// <returns>True if another conflicting mod is loaded</returns>
        public bool ConflictingModsLoaded() {
            return _conflictingMods.Any(m => m.LoadState != ResourceLoadState.Unloaded);
        }

        /// <summary>
        ///     Is another conflicting Mod enabled?
        /// </summary>
        /// <returns>True if another conflicting mod is enabled</returns>
        public bool ConflictingModsEnabled() {
            return _conflictingMods.Any(m => m.IsEnabled);
        }

        /// <summary>
        ///     Invalidate the mod
        /// </summary>
        public void SetInvalid() {
            IsValid = false;
        }

        /// <summary>
        ///     Get an asset with name.
        /// </summary>
        /// <param name="name">The asset's name.</param>
        /// <returns>The asset if it has been found. Null otherwise</returns>
        public Object GetAsset(string name) {
            if (_assetsResource.LoadState == ResourceLoadState.Loaded) {
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
            if (_assetsResource.LoadState == ResourceLoadState.Loaded) {
                return _assetsResource.AssetBundle.LoadAsset<T>(name);
            }

            return null;
        }

        /// <summary>
        ///     Get all assets of a certain Type.
        /// </summary>
        /// <typeparam name="T">The asset Type.</typeparam>
        /// <returns>AssetBundleRequest that can be used to get the asset.</returns>
        public T[] GetAssets<T>() where T : Object {
            if (_assetsResource.LoadState == ResourceLoadState.Loaded) {
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
        public AssetBundleRequest GetAssetAsync<T>(string name) where T : Object {
            if (_assetsResource.LoadState == ResourceLoadState.Loaded) {
                return _assetsResource.AssetBundle.LoadAssetAsync<T>(name);
            }

            return null;
        }

        /// <summary>
        ///     Get all assets of a certain Type.
        /// </summary>
        /// <typeparam name="T">The asset Type.</typeparam>
        /// <returns>AssetBundleRequest that can be used to get the assets.</returns>
        public AssetBundleRequest GetAssetsAsync<T>() where T : Object {
            if (_assetsResource.LoadState == ResourceLoadState.Loaded) {
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

            if (LoadState != ResourceLoadState.Loaded) {
                return instances.ToArray();
            }

            foreach (var assembly in _runtimeAssemblies) {
                try {
                    instances.AddRange(GetInstances<T>(assembly, args));
                }
                catch (Exception e) {
                    LogUtility.LogException(e);
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

                if (_allInstances.TryGetValue(type, out var foundInstance)) {
                    //LogUtility.Log("existing instance of " + typeof(T).Name + " found: " + type.Name);
                    instances.Add((T) foundInstance);
                    continue;
                }

                if (type.IsSubclassOf(typeof(Component))) {
                    foreach (var component in GetComponents(type)) {
                        instances.Add((T) (object) component);
                    }

                    continue;
                }

                try {
                    var instance = (T) Activator.CreateInstance(type, args);
                    instances.Add(instance);
                    _allInstances.Add(type, instance);
                }
                catch (Exception e) {
                    if (e is MissingMethodException) {
                        Debug.Log(e.Message);
                    }
                    else {
                        LogUtility.LogException(e);
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

    }

}