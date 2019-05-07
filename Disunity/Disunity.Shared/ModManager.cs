using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;


namespace Disunity.Core {

    /// <summary>
    ///     Main class for finding mods
    /// </summary>
    public sealed class ModManager<T> where T : Mod {

        private List<string> _modPaths = new List<string>();

        public ModManager() {
            _mods = new List<T>();
            _searchDirectories = new List<string>();

            Mods = _mods.AsReadOnly();
        }

        /// <summary>
        /// Mods that have been loaded.
        /// </summary>
        private List<T> _mods;
        
        /// <summary>
        /// 
        /// </summary>
        private List<string> _searchDirectories;

        /// <summary>
        ///     All mods that have been found in all search directories.
        /// </summary>
        public ReadOnlyCollection<T> Mods { get; private set; }

        /// <inheritdoc />


        /// <summary>
        ///     Add a directory that will be searched for Mods
        /// </summary>
        /// <param name="path">The path of the search directory.</param>
        public void AddSearchDirectory(string path) {
            if (_searchDirectories.Any(s => s.NormalizedPath() == path.NormalizedPath())) {
                return;
            }

            if (!Directory.Exists(path)) {
                throw new DirectoryNotFoundException(path);
            }

            _searchDirectories.Add(path);
        }

        private List<string> GetModInfoPaths() {
            var paths = new List<string>();
            foreach (var path in _searchDirectories) {
                var files = Directory.GetFiles(path, "*.info", SearchOption.AllDirectories);
                paths.AddRange(files);
            }

            return paths;
        }

        public void DiscoverMods() {
            var modPaths = GetModInfoPaths();
            foreach (var path in modPaths) {
                var mod = (T) Activator.CreateInstance(typeof(T), path);
                _modPaths.Add(path);
                AddMod(mod);
            }
        }

        private void AddMod(T mod) {

            //mod.UpdateConflicts(_mods);
            //foreach (var other in _mods) {
            //    other.UpdateConflicts(mod);
            //}

            _mods.Add(mod);
        }
    }
}