using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using Disunity.Core;
using Path = System.IO.Path;


namespace Disunity.Runtime {

    [BepInPlugin("com.disunity.plugin", "Disunity", "0.1")]
    public class BepinPlugin : BaseUnityPlugin {

        private readonly List<RuntimeMod> _instances = new List<RuntimeMod>();
        private readonly RuntimeLogger _log = new RuntimeLogger("Disunity Runtime");

        private void Awake() {
            var searchDirectory = Path.Combine(Paths.PluginPath, "../mods");
            ModFinder.Find(searchDirectory).ToList().ForEach(LoadMod);
        }

        private void Start() {
            foreach (RuntimeMod mod in _instances) {
                mod.InvokeOnStart();
            }
        }

        private void Update() {
            foreach (RuntimeMod mod in _instances) {
                mod.InvokeOnUpdate();
            }
        }

        private static Assembly GetStartupAssembly(string name) {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(a => a.GetName().Name == name);
        }

        private static Type GetStartupClass(Assembly assembly, string name) {
            return assembly.GetType(name);
        }

        private void InstantiateStartupClass(Type type, RuntimeMod mod) {
            Activator.CreateInstance(type, mod);
            _instances.Add(mod);
        }

        private void BootMod(RuntimeMod mod) {
            var startupAssembly = mod.Info.RuntimeAssembly;
            var startupClass = mod.Info.RuntimeClass;

            if (startupAssembly.IsNullOrWhiteSpace() || startupClass.IsNullOrWhiteSpace()) {
                _log.LogInfo($"Loaded {mod.Info.Name}");
                return;
            }

            var assembly = GetStartupAssembly(startupAssembly);
            if (assembly == null) {
                _log.LogInfo($"Couldn't find runtime startup assembly for {mod.Info.Name}: {startupAssembly}");
                return;
            }

            var type = GetStartupClass(assembly, startupClass);
            if (type == null) {
                _log.LogInfo($"Couldn't find runtime startup class for {mod.Info.Name} in assembly {startupAssembly}: {startupClass}");
                return;
            }

            InstantiateStartupClass(type, mod);
            _log.LogInfo($"Loaded {mod.Info.Name} {mod.Info.Version} by {mod.Info.Author}");
        }

        private void LoadMod(string modInfoPath) {
            var mod = new RuntimeMod(modInfoPath);

            if (!mod.IsValid) return;

            BootMod(mod);
        }
    }
}