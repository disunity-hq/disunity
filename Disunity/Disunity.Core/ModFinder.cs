using System;
using System.Collections.Generic;
using System.IO;


namespace Disunity.Core {

    /// <summary>
    ///     Main class for finding mods
    /// </summary>
    public static class ModFinder {

        public static HashSet<string> Find(HashSet<string> searchDirectories) {
            var paths = new HashSet<string>();
            foreach (var path in searchDirectories) {
                var files = Directory.GetFiles(path, "*.info", SearchOption.AllDirectories);
                paths.UnionWith(files);
            }
            return paths;
        }

        public static HashSet<string> Find(string searchDirectory) {
            var searchDirectories = new HashSet<string>() { searchDirectory };
            return Find(searchDirectories);
        }
    }
}