using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using BepInEx;

using Disunity.Core;

using Mono.Cecil;


namespace Disunity.Preloader {

    public class Preloader {

        private static readonly List<PreloadMod> _instances = new List<PreloadMod>();
        private static readonly PreloadLogger _log = new PreloadLogger("Disunity Preload");
        private static Dictionary<string, List<Action<AssemblyDefinition>>> patchers = new Dictionary<string, List<Action<AssemblyDefinition>>>();

        public static IEnumerable<string> TargetDLLs {
            get {
                LoadMods();

                foreach (var key in patchers.Keys) {
                    yield return $"{key}.dll";
                }
            }
        }

        public static void RegisterPatcher(string assemblyName, Action<AssemblyDefinition> patcher) {
            List<Action<AssemblyDefinition>> assemblyPatchers;

            if (patchers.ContainsKey(assemblyName)) {
                assemblyPatchers = patchers[assemblyName];
            } else {
                assemblyPatchers = new List<Action<AssemblyDefinition>>();
            }

            assemblyPatchers.Add(patcher);
            patchers[assemblyName] = assemblyPatchers;
        }

        public static void Patch(ref AssemblyDefinition assembly) {
            if (!patchers.TryGetValue(assembly.Name.Name, out var assemblyPatchers)) {
                return;
            }

            foreach (var patcher in assemblyPatchers) {
                try {
                    patcher(assembly);
                }
                catch (Exception e) {
                    _log.LogError($"Failed to run patch {patcher.Method.Name}. Error:\n{e}");
                }
            }
        }

        protected static Assembly GetStartupAssembly(PreloadMod mod) {
            var assemblyName = mod.Info.PreloadAssembly;
            var assemblyPath = Path.Combine(mod.InstallPath, "preload", $"{assemblyName}.dll");
            return Assembly.LoadFrom(assemblyPath);
        }

        protected static Type GetStartupClass(Assembly assembly, string name) {
            return assembly.GetType(name);
        }

        protected static void InstantiateStartupClass(Type type, PreloadMod mod) {
            Activator.CreateInstance(type, mod);
            _instances.Add(mod);
        }

        protected static void BootMod(PreloadMod mod) {
            var startupAssembly = mod.Info.PreloadAssembly;
            var startupClass = mod.Info.PreloadClass;

            if (startupAssembly.IsNullOrWhiteSpace() || startupClass.IsNullOrWhiteSpace()) {
                _log.LogInfo($"Loaded {mod.Info.Name}");
                return;
            }

            var assembly = GetStartupAssembly(mod);

            if (assembly == null) {
                _log.LogInfo($"Couldn't find preload startup assembly for {mod.Info.Name}: {startupAssembly}");
                return;
            }

            var type = GetStartupClass(assembly, startupClass);

            if (type == null) {
                _log.LogInfo($"Couldn't find preload startup class for {mod.Info.Name} in assembly {startupAssembly}: {startupClass}");
                return;
            }

            InstantiateStartupClass(type, mod);
            _log.LogInfo($"Loaded {mod.Info.Name} {mod.Info.Version} by {mod.Info.Author}");
        }

        protected static void LoadMod(string modInfoPath) {
            var mod = new PreloadMod(modInfoPath);

            if (!mod.IsValid) {
                _log.LogWarning($"Failed to load {mod.Info.Name}");
                return;
            }

            BootMod(mod);
        }

        protected static void LoadMods() {
            var searchDirectory = Path.Combine(Paths.PatcherPluginPath, "../mods");
            _log.LogInfo($"Searching for mods in {searchDirectory}");

            ModFinder.Find(searchDirectory).ToList().ForEach(LoadMod);
        }

    }

}