using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using Disunity.Core;
using Disunity.Interface;
using Disunity.Preloader;
using Mono.Cecil;
using BepInLogger = BepInEx.Logging.Logger;


namespace Disunity.PreloaderPatcher {

    internal static class DisunityPatcher {

        private static PreloaderPatch currentPatcher = null;
        public static ManualLogSource Logger = BepInLogger.CreateLogSource("Disunity Preloader");
        private static ModManager<PreloaderMod> modManager = new ModManager<PreloaderMod>();

        public static IEnumerable<string> TargetDLLs {
            get {
                foreach (var mod in modManager.Mods) {
                    foreach (var preloaderPatch in mod.Patches) {
                        currentPatcher = preloaderPatch;

                        foreach (string targetAssembly in preloaderPatch.TargetAssemblies) {
                            yield return targetAssembly;
                        }
                    }
                }
            }
        }

        public static void Patch(ref AssemblyDefinition assembly) {
            if (currentPatcher == null)
                return;

            Logger.LogInfo($"Patch: {currentPatcher.GetType().FullName} [{assembly.Name}]");

            try {
                currentPatcher.Patch(ref assembly);
            }
            catch (Exception e) {
                Logger.LogError($"Failed to run patch {currentPatcher.GetType().FullName}. Error:\n{e}");
            }
        }

        public static void Finish() {
            foreach (var mod in modManager.Mods) {
                try {
                    mod.FinalizePatchers();
                }
                catch (Exception e) {
                    Logger.LogError($"Failed run finalizer on {currentPatcher.GetType().FullName}. Error:\n{e}");
                }
            }

            currentPatcher = null;
            modManager = null;

            Logger.LogInfo("Disunity Preloader completed");
            BepInLogger.Sources.Remove(Logger);
            Logger.Dispose();
            Logger = null;
        }

        public static void Initialize() {
            Logger.LogInfo("Disunity Preloader starting...");

            //TODO: Move Disunity mods location to a global "Paths" class for both plugin and preloader use
            string searchDir = Path.Combine(Paths.BepInExRootPath, "mods");

            Logger.LogInfo($"Searching for mods in {searchDir}");

            modManager.AddSearchDirectory(searchDir);
            modManager.DiscoverMods();
        }

    }

}