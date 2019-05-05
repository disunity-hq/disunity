using System.Linq;
using BepInEx;
using Disunity.Core;
using Disunity.Interface;
using Disunity.Shared;
using On.RoR2;
using UnityEngine;
using Path = System.IO.Path;


namespace Disunity.Plugin {

    [BepInPlugin("com.disunity.plugin", "Disunity", "0.1")]
    public class BepinPlugin : BaseUnityPlugin {

        public void Awake() {
            RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };

            Debug.Log("Disunity Runtime 0.0.1 Starting...");

            var searchDirectory = Path.Combine(Paths.PluginPath, "../mods");
            var mm = ModManager.Instance;
            mm.AddSearchDirectory(searchDirectory);
            mm.RefreshSearchDirectories();
            Debug.Log($"Disunity search path: {searchDirectory}");

            mm.ModFound += mod => {
                Debug.Log(
                    $"Disunity mod found: {mod.Name} {mod.ModInfo.Version} by {mod.ModInfo.Author}");

                mod.Load();
            };

            mm.ModLoaded += mod => {
                Debug.Log($"Disunity mod loaded: {mod.Name}");
            };

        }

    }

}