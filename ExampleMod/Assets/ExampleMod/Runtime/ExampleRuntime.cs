using System.Linq;

using UnityEngine;
using Disunity.Runtime;

public class ExampleRuntime
{

    private readonly RuntimeMod _mod;
    private readonly RuntimeLogger _log;

    public ExampleRuntime(RuntimeMod mod)
    {
        _mod = mod;
        _mod.OnStart += (s, a) => Start();

        _log = mod.Log;
        _log.LogInfo("Hello from ExampleRuntime.");
    }

    void Start() {
        var prefab = _mod.Prefabs.FirstOrDefault(p => p.name == "ExampleUI");
        Object.Instantiate(prefab);
    }
}