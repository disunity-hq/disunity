using Disunity.Interface;
using UnityEngine;

public class ExampleMod : ModBehaviour
{
    public GameObject prefab;
    public override void OnLoaded(ContentHandler contentHandler)
    {
        Debug.Log("Hello World!!!!!!!!!???");

        // unlock all the things
        On.RoR2.UserProfile.HasSurvivorUnlocked += (o, s, i) => true;
        On.RoR2.UserProfile.HasDiscoveredPickup += (o, s, i) => true;
        On.RoR2.UserProfile.HasAchievement += (o, s, i) => true;
        On.RoR2.UserProfile.CanSeeAchievement += (o, s, i) => true;
        On.RoR2.UserProfile.HasUnlockable_UnlockableDef += (o, s, i) => true;

        // get reference to game's main canvas
        var canvas = RoR2.RoR2Application.instance.mainCanvas;

        // instantiate UI prefab
        var gobj = Instantiate(prefab);
        gobj.transform.SetParent(canvas.transform, false);

        // set the parent to game's canvas and fix the sizings
        var rect = gobj.GetComponent<RectTransform>();
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        rect.anchorMin = new Vector2(0.00f, 0.00f);
        rect.anchorMax = new Vector2(1.00f, 1.00f);
    }
}