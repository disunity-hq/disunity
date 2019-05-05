using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;


namespace Disunity.Core {

    /// <summary>
    ///     Represents a directory that is monitored for Mods.
    /// </summary>
    /// <remarks>
    ///     Opens a thread that watches the directory.
    /// </remarks>
    public class ModSearchDirectory : IDisposable {

        private readonly Dictionary<string, long> _modPaths;

        private readonly Thread _backgroundRefresh;
        private readonly AutoResetEvent _refreshEvent;
        private bool _disposed;

        /// <summary>
        ///     Initialize a new ModSearchDirectory with a path.
        /// </summary>
        /// <param name="path">The path to the search directory.</param>
        public ModSearchDirectory(string path) {
            Path = System.IO.Path.GetFullPath(path);

            if (!Directory.Exists(Path)) {
                throw new DirectoryNotFoundException(Path);
            }

            _modPaths = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

            _refreshEvent = new AutoResetEvent(false);

            _backgroundRefresh = new Thread(BackgroundRefresh);
            _backgroundRefresh.Start();
        }

        /// <summary>
        ///     This ModSearchDirectory's path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Releases all resources used by the ModSearchDirectory.
        /// </summary>
        public void Dispose() {
            ModFound = null;
            ModRemoved = null;
            ModChanged = null;

            _disposed = true;
            _refreshEvent.Set();
            _backgroundRefresh.Join();
        }

        /// <summary>
        ///     Occurs when a new Mod has been found.
        /// </summary>
        public event Action<string> ModFound;

        /// <summary>
        ///     Occurs when a Mod has been removed.
        /// </summary>
        public event Action<string> ModRemoved;

        /// <summary>
        ///     Occurs when a change to a Mod's directory has been detected.
        /// </summary>
        public event Action<string> ModChanged;

        /// <summary>
        ///     Occurs when any change was detected for any Mod in this search directory.
        /// </summary>
        public event Action ModsChanged;

        /// <summary>
        ///     Refresh the collection of mod paths. Remove all missing paths and add all new paths.
        /// </summary>
        public void Refresh() {
            _refreshEvent.Set();
        }

        private void BackgroundRefresh() {
            Thread.CurrentThread.IsBackground = true;

            _refreshEvent.WaitOne();

            while (!_disposed) {
                DoRefresh();

                _refreshEvent.WaitOne();
            }
        }

        private bool ModDirectoryChanged(DirectoryInfo modDirectory, string path, long currentTicks, long lastWriteTime) {
            if (modDirectory.LastWriteTime.Ticks <= lastWriteTime) {
                return false;
            }

            _modPaths[path] = currentTicks;
            UpdateModPath(path);
            return true;
        }

        private bool ModDirectoriesChanged(DirectoryInfo modDirectory, string path, long currentTicks, long lastWriteTime) {

            var updatedDirectories = modDirectory
                .GetDirectories("*", SearchOption.AllDirectories)
                .Any(directory => directory.LastWriteTime.Ticks > lastWriteTime);

            if (!updatedDirectories) {
                return false;
            }

            _modPaths[path] = currentTicks;
            UpdateModPath(path);
            return true;
        }

        private bool ModFilesChanged(DirectoryInfo modDirectory, string path, long currentTicks, long lastWriteTime) {
            var updatedFiles = modDirectory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => file.Extension != ".info")
                .Any(file => file.LastWriteTime.Ticks > lastWriteTime);

            if (!updatedFiles) {
                return false;
            }

            _modPaths[path] = currentTicks;
            UpdateModPath(path);
            return true;
        }

        private bool ExistingModsChanged(string[] modInfoPaths) {
            var changed = false;

            foreach (var path in _modPaths.Keys.ToArray()) {
                if (!modInfoPaths.Contains(path)) {
                    changed = true;
                    RemoveModPath(path);
                    continue;
                }

                var modDirectory = new DirectoryInfo(System.IO.Path.GetDirectoryName(path));
                var currentTicks = DateTime.Now.Ticks;
                var lastWriteTime = _modPaths[path];

                if (ModDirectoryChanged(modDirectory, path, currentTicks, lastWriteTime)) {
                    changed = true;
                    continue;
                }

                if (ModDirectoriesChanged(modDirectory, path, currentTicks, lastWriteTime)) {
                    changed = true;
                    continue;
                }

                if (ModFilesChanged(modDirectory, path, currentTicks, lastWriteTime)) {
                    changed = true;
                }
            }

            return changed;
        }

        private bool WereModsAdded(string[] modInfoPaths) {
            var changed = false;

            foreach (var path in modInfoPaths) {
                if (_modPaths.ContainsKey(path)) {
                    continue;
                }

                changed = true;
                AddModPath(path);
            }

            return changed;
        }

        private void DoRefresh() {

            var modInfoPaths = GetModInfoPaths();

            var changed = ExistingModsChanged(modInfoPaths);

            changed |= WereModsAdded(modInfoPaths);

            if (changed) {
                ModsChanged?.Invoke();
            }
        }

        private void AddModPath(string path) {
            if (_modPaths.ContainsKey(path)) {
                return;
            }

            _modPaths.Add(path, DateTime.Now.Ticks);

            ModFound?.Invoke(path);
        }

        private void RemoveModPath(string path) {
            if (!_modPaths.ContainsKey(path)) {
                return;
            }

            _modPaths.Remove(path);
            ModRemoved?.Invoke(path);
        }

        private void UpdateModPath(string path) {
            if (!File.Exists(path)) {
                RemoveModPath(path);
                return;
            }

            ModChanged?.Invoke(path);
        }

        private string[] GetModInfoPaths() {
            return Directory.GetFiles(Path, "*.info", SearchOption.AllDirectories);
        }

    }

}