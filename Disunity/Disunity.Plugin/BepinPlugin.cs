using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using Disunity.Core;
using Path = System.IO.Path;


namespace Disunity.Runtime {

    [BepInPlugin("com.disunity.plugin", "Disunity", "0.1")]
    public class BepinPlugin : BaseUnityPlugin {

        private List<RuntimeMod> instances = new List<RuntimeMod>();

        private void Awake() {

            var log = BepInEx.Logging.Logger.CreateLogSource("Disunity");

            log.LogInfo("RUNTIME stage starting...");

            var searchDirectory = Path.Combine(Paths.PluginPath, "../mods");

            var mm = new ModManager<RuntimeMod>();
            mm.AddSearchDirectory(searchDirectory);
            mm.DiscoverMods();

            foreach (var mod in mm.Mods) {
                var startupAssembly = mod.Info.StartupAssembly;
                var startupClass = mod.Info.StartupClass;

                if (startupAssembly.IsNullOrWhiteSpace() || startupClass.IsNullOrWhiteSpace()) {
                    log.LogInfo($"Loaded {mod.Info.Name}");
                    continue;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == startupAssembly);

                if (assembly == null) {
                    log.LogInfo($"Couldn't find startup assembly for {mod.Info.Name}: {startupAssembly}");
                    continue;
                }

                var type = assembly.GetType(startupClass);

                if (type == null) {
                    log.LogInfo($"Couldn't find startup class for {mod.Info.Name} in assembly {startupAssembly}: {startupClass}");
                    continue;
                }


                RuntimeLogger logger = new RuntimeLogger(mod.Info.Name);
                Activator.CreateInstance(type, mod, logger);
                instances.Add(mod);

                log.LogInfo($"Loaded {mod.Info.Name}");
            }
        }

        private void Start() {
            foreach (RuntimeMod mod in instances) {
                mod.InvokeOnStart();
            }
        }

        private void Update() {
            foreach (RuntimeMod mod in instances) {
                mod.InvokeOnUpdate();
            }
        }
    }
}