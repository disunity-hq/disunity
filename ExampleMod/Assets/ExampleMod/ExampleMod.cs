using System.Linq;

using UnityEngine;
using Disunity.Core;
using Disunity.Runtime;


public class ExampleMod
{

    private readonly RuntimeMod _mod;
    private readonly RuntimeLogger _log;

    public ExampleMod(RuntimeMod mod)
    {
        _mod = mod;
        _log = mod.Log;

        _log.LogInfo("Hello from Example Mod!?");

        _mod.OnStart += (s, a) => Start();

        foreach (var artifact in _mod.Artifacts.Keys) {
            _log.LogInfo($"Found artifact: {artifact}");
        }
    }

    void Start() {
        var prefab = _mod.Prefabs.FirstOrDefault(p => p.name == "ExampleUI");

        if (prefab == null) {
            _log.LogError("Prefab was null, aborting.");
            return;
        }

        Object.Instantiate(prefab);
    }
}