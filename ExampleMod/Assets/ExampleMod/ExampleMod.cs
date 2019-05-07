using System.Linq;

using UnityEngine;
using RoR2;
using Disunity.Runtime;


public class ExampleMod
{
    RuntimeMod _mod;

    public ExampleMod(RuntimeMod mod)
    {
        Debug.Log("Hello from Example Mod!");

        _mod = mod;

        _mod.OnStart += (s, a) => Start();

        // unlock all the things
        On.RoR2.UserProfile.HasSurvivorUnlocked += (o, s, i) => true;
        On.RoR2.UserProfile.HasDiscoveredPickup += (o, s, i) => true;
        On.RoR2.UserProfile.HasAchievement += (o, s, i) => true;
        On.RoR2.UserProfile.CanSeeAchievement += (o, s, i) => true;
        On.RoR2.UserProfile.HasUnlockable_UnlockableDef += (o, s, i) => true;

    }

    void Start() {
        // get reference to game's main canvas
        var canvas = RoR2Application.instance.mainCanvas;

        var prefab = _mod.Prefabs.Where(p => p.name == "ExampleUI").FirstOrDefault();

        if (prefab == null) {
            Debug.Log("Prefab was null, aborting.");
            return;
        }

        // instantiate UI prefab
        var gobj = GameObject.Instantiate(prefab);
        gobj.transform.SetParent(canvas.transform, false);

        // set the parent to game's canvas and fix the sizings
        var rect = gobj.GetComponent<RectTransform>();
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        rect.anchorMin = new Vector2(0.00f, 0.00f);
        rect.anchorMax = new Vector2(1.00f, 1.00f);
    }
}