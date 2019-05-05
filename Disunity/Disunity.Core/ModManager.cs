using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using Disunity.Shared;
using UnityEngine;


namespace Disunity.Core {

    /// <summary>
    ///     Main class for finding mods
    /// </summary>
    public sealed class ModManager {

        static ModManager() { }

        private static ModManager _instance;

        public static ModManager Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                _instance = new ModManager();

                return _instance;
            }
        }

        /// <summary>
        /// Locks access to the Mod paths.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Paths of loaded mods.
        /// </summary>
        private Dictionary<string, Mod> _modPaths;

        /// <summary>
        /// Mods that have been loaded.
        /// </summary>
        private List<Mod> _mods;

        /// <summary>
        /// How often to scan search directories.
        /// </summary>
        private int _refreshInterval;

        /// <summary>
        /// Allows manager to run co-routines on the main thread.
        /// </summary>
        private Dispatcher _dispatcher;

        /// <summary>
        /// Unused.
        /// </summary>
        public Dictionary<GameObject, Mod> ModMap;
        private List<Mod> _queuedRefreshMods;

        private List<ModSearchDirectory> _searchDirectories;
        private WaitForSeconds _wait;

        /// <summary>
        ///     Default directory that will be searched for mods.
        /// </summary>
        public string DefaultSearchDirectory { get; private set; }

        /// <summary>
        ///     The interval (in seconds) between refreshing Mod search directories.
        ///     Set to 0 to disable auto refreshing.
        /// </summary>
        //public int RefreshInterval {
        //    get => _refreshInterval;
        //    set {
        //        _refreshInterval = value;

        //        StopAllCoroutines();

        //        if (_refreshInterval < 1) {
        //            return;
        //        }

        //        _wait = new WaitForSeconds(_refreshInterval);
        //        StartCoroutine(AutoRefreshSearchDirectories());
        //    }
        //}

        /// <summary>
        ///     All mods that have been found in all search directories.
        /// </summary>
        public ReadOnlyCollection<Mod> Mods { get; private set; }

        /// <summary>
        ///     Occurs when the collection of Mods has changed.
        /// </summary>
        public event Action ModsChanged;

        /// <summary>
        ///     Occurs when a Mod has been found.
        /// </summary>
        public event Action<Mod> ModFound;

        /// <summary>
        ///     Occurs when a Mod has been removed. The Mod will be marked invalid.
        /// </summary>
        public event Action<Mod> ModRemoved;

        /// <summary>
        ///     Occurs when a Mod has been loaded
        /// </summary>
        public event Action<Mod> ModLoaded;

        /// <summary>
        ///     Occurs when a Mod has been Unloaded
        /// </summary>
        public event Action<Mod> ModUnloaded;

        /// <summary>
        ///     Occurs when a Mod has cancelled async loading
        /// </summary>
        public event Action<Mod> ModLoadCancelled;

        /// <summary>
        ///     Occurs when a ModScene has been loaded
        /// </summary>
        public event Action<ModScene> SceneLoaded;

        /// <summary>
        ///     Occurs when a ModScene has been unloaded
        /// </summary>
        public event Action<ModScene> SceneUnloaded;

        /// <summary>
        ///     Occurs when a ModScene has cancelled async loading
        /// </summary>
        public event Action<ModScene> SceneLoadCancelled;

        /// <inheritdoc />
        public ModManager() {
            LogUtility.LogLevel = LogLevel.Debug;

            _dispatcher = Dispatcher.Instance;

            lock (_lock) { _modPaths = new Dictionary<string, Mod>(); }

            _mods = new List<Mod>();
            _queuedRefreshMods = new List<Mod>();
            _searchDirectories = new List<ModSearchDirectory>();

            Mods = _mods.AsReadOnly();

            DefaultSearchDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Mods");

            if (!Directory.Exists(DefaultSearchDirectory)) {
                Directory.CreateDirectory(DefaultSearchDirectory);
            }

            AddSearchDirectory(DefaultSearchDirectory);
        }

        private void OnModLoaded(Resource mod) {
            ModLoaded?.Invoke((Mod) mod);
        }

        private void OnModUnloaded(Resource modResource) {
            var mod = (Mod) modResource;

            ModUnloaded?.Invoke(mod);

            if (!_queuedRefreshMods.Contains(mod)) {
                return;
            }

            _queuedRefreshMods.Remove(mod);
            OnModChanged(mod.ModInfo.Path);
        }

        private void OnModLoadCancelled(Resource mod) {
            ModLoadCancelled?.Invoke((Mod) mod);
        }

        private void OnSceneLoaded(ModScene scene) {
            SceneLoaded?.Invoke(scene);
        }

        private void OnSceneUnloaded(ModScene scene) {
            SceneUnloaded?.Invoke(scene);
        }

        private void OnSceneLoadCancelled(ModScene scene) {
            SceneLoadCancelled?.Invoke(scene);
        }

        /// <summary>
        ///     Add a directory that will be searched for Mods
        /// </summary>
        /// <param name="path">The path of the search directory.</param>
        public void AddSearchDirectory(string path) {
            if (_searchDirectories.Any(s => s.Path.NormalizedPath() == path.NormalizedPath())) {
                return;
            }

            var directory = new ModSearchDirectory(path);

            directory.ModFound += OnModFound;
            directory.ModRemoved += OnModRemoved;
            directory.ModChanged += OnModChanged;

            _searchDirectories.Add(directory);

            directory.Refresh();
        }

        /// <summary>
        ///     Remove a directory that will be searched for mods
        /// </summary>
        /// <param name="path">The path of the search directory.</param>
        public void RemoveSearchDirectory(string path) {
            var directory = _searchDirectories.Find(s => s.Path.NormalizedPath() == path.NormalizedPath());

            if (directory == null) {
                return;
            }

            directory.Dispose();

            _searchDirectories.Remove(directory);
        }

        /// <summary>
        ///     Refresh all search directories and update any new, changed or removed Mods.
        /// </summary>
        public void RefreshSearchDirectories() {
            foreach (var searchDirectory in _searchDirectories) {
                searchDirectory.Refresh();
            }
        }

        private IEnumerator AutoRefreshSearchDirectories() {
            while (true) {
                RefreshSearchDirectories();
                yield return _wait;
            }
        }

        private void OnModFound(string path) {
            //AddMod(path);
            ThreadPool.QueueUserWorkItem(o => AddMod(path));
        }

        private void OnModRemoved(string path) {
            RemoveMod(path);
        }

        private void OnModChanged(string path) {
            RefreshMod(path);
        }

        private void RefreshMod(string path) {
            LogUtility.LogInfo("Mod refreshing: " + path);
            OnModRemoved(path);
            OnModFound(path);
        }

        private void QueueModRefresh(Mod mod) {
            if (_queuedRefreshMods.Contains(mod)) {
                return;
            }

            LogUtility.LogInfo("Mod refresh queued: " + mod.Name);
            mod.SetInvalid();
            _queuedRefreshMods.Add(mod);
        }

        private void AddMod(string path) {
            lock (_lock) {
                if (_modPaths.ContainsKey(path)) {
                    return;
                }
            }

            var mod = new Mod(path);

            lock (_lock) {
                _modPaths.Add(path, mod);
            }

            _dispatcher.Enqueue(() => AddMod(mod), true);
        }

        private void AddMod(Mod mod) {
            mod.Loaded += OnModLoaded;
            mod.Unloaded += OnModUnloaded;
            mod.LoadCancelled += OnModLoadCancelled;
            mod.SceneLoaded += OnSceneLoaded;
            mod.SceneUnloaded += OnSceneUnloaded;
            mod.SceneLoadCancelled += OnSceneLoadCancelled;

            mod.UpdateConflicts(_mods);
            foreach (var other in _mods) {
                other.UpdateConflicts(mod);
            }

            LogUtility.LogInfo("Mod found: " + mod.Name + " - " + mod.ContentTypes);
            _mods.Add(mod);

            ModFound?.Invoke(mod);
            ModsChanged?.Invoke();
        }

        private void RemoveMod(string path) {
            lock (_lock) {
                if (!_modPaths.TryGetValue(path, out var mod)) {
                    return;
                }

                if (mod.LoadState != ResourceLoadState.Unloaded) {
                    _dispatcher.Enqueue(() => QueueModRefresh(mod));
                    return;
                }

                _modPaths.Remove(path);

                _dispatcher.Enqueue(() => RemoveMod(mod), true);
            }
        }

        private void RemoveMod(Mod mod) {
            mod.Loaded -= OnModLoaded;
            mod.Unloaded -= OnModUnloaded;
            mod.LoadCancelled -= OnModLoadCancelled;
            mod.SceneLoaded -= OnSceneLoaded;
            mod.SceneUnloaded -= OnSceneUnloaded;
            mod.SceneLoadCancelled -= OnSceneLoadCancelled;
            mod.SetInvalid();

            foreach (var other in _mods) {
                other.UpdateConflicts(mod);
            }

            LogUtility.LogInfo("Mod removed: " + mod.Name);
            _mods.Remove(mod);

            ModRemoved?.Invoke(mod);
            ModsChanged?.Invoke();
        }


        /// <inheritdoc />
        protected void OnDestroy() {
            _queuedRefreshMods.Clear();

            foreach (var mod in _mods) {
                mod.Unload();
                mod.SetInvalid();
            }

            foreach (var searchDirectory in _searchDirectories) {
                searchDirectory.Dispose();
            }
        }
    }
}