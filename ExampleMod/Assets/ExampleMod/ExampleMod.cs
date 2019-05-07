using System.Linq;

using UnityEngine;
using Disunity.Runtime;


public class ExampleMod
{
    RuntimeMod _mod;
    RuntimeLogger _log;

    public ExampleMod(RuntimeMod mod)
    {
        _mod = mod;
        _log = mod.Log;

        _log.LogInfo("Hello from Example Mod!");

        _mod.OnStart += (s, a) => Start();
    }

    void Start() {
        var prefab = _mod.Prefabs.Where(p => p.name == "ExampleUI").FirstOrDefault();

        if (prefab == null) {
            _log.LogError("Prefab was null, aborting.");
            return;
        }

        GameObject.Instantiate(prefab);
    }
}