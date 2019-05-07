using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Disunity.Core;
using Disunity.Interface;
using Disunity.PreloaderPatcher;


namespace Disunity.Preloader {

    internal class PreloaderMod : Mod {

        public List<PreloaderPatch> Patches { get; } = new List<PreloaderPatch>();

        public PreloaderMod(string infoPath) : base(infoPath) { LoadPatchers(); }

        private void LoadPatchers() {
            foreach (string assPath in Info.PreloadAssemblies.Select(p => Path.Combine(InstallPath, p))) {
                if (!File.Exists(assPath)) {
                    continue;
                }

                var assemblyPatchers = LoadPreloaderPatchers(assPath);

                foreach (var preloaderPatch in assemblyPatchers) {
                    try {
                        preloaderPatch.OnInitialize();
                        Patches.Add(preloaderPatch);
                    }
                    catch (Exception e) {
                        DisunityPatcher.Logger.LogError($"Error while initializing {preloaderPatch.GetType().FullName} from {Info.Name}. Error:\n{e}");
                    }
                }
            }
        }

        public void FinalizePatchers() {
            foreach (var preloaderPatch in Patches) {
                preloaderPatch.OnFinalize();
            }
        }

        private IEnumerable<PreloaderPatch> LoadPreloaderPatchers(string assemblyPath) {
            var result = new List<PreloaderPatch>();
            try {
                var assembly = Assembly.LoadFile(assemblyPath);

                result.AddRange(assembly.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof(PreloaderPatch).IsAssignableFrom(t)).Select(patchType => (PreloaderPatch)Activator.CreateInstance(patchType)));
            }
            catch (BadImageFormatException) { } // DLL is unmanaged; weird but OK
            catch (ReflectionTypeLoadException re) {
                DisunityPatcher.Logger.LogError($"Cannot load {Path.GetFileNameWithoutExtension(assemblyPath)} due to missing dependencies.");
                DisunityPatcher.Logger.LogError(DumpReflectionTypeLoadInfo(re));
            }
            catch (Exception e) { }

            return result;
        }

        private static string DumpReflectionTypeLoadInfo(ReflectionTypeLoadException ex) {
            var sb = new StringBuilder();
            foreach (var loaderEx in ex.LoaderExceptions) {
                sb.AppendLine(loaderEx.Message);
                switch (loaderEx) {
                    case FileNotFoundException exFileNotFound: {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
                            sb.AppendLine("Load log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }

                        break;
                    }

                    case FileLoadException exLoad: {
                        if (!string.IsNullOrEmpty(exLoad.FusionLog)) {
                            sb.AppendLine("Load log:");
                            sb.AppendLine(exLoad.FusionLog);
                        }

                        break;
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }

}