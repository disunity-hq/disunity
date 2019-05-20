using System.Linq;
using UnityEditor;


namespace Disunity.Editor.Editors {

    internal class PrefabEditor : BaseAssetEditor {

        public PrefabEditor(EditorWindow window) : base(window) { }

        public override string Label() {
            return "Prefabs";
        }

        public override string Title() {
            return "Premade GameObject prefabs";
        }

        public override string Help() {
            return @"Prefabs and ScriptableObjects can be exported here.

You can access your prefabs via the `Mod.Prefabs` attribute. ScriptableObjects
work exactly the same way, except they can't be instantiated into the scene.

<b>ScriptableObjects</b>

ScriptableObjects are non-Scene-objects which can contain data. This is useful
for all sorts of things including settings. However, a limitation of Disunity
(and other mod solutions for Unity) is that you can only use basic data-types
for fields on a ScriptableObject. Normally, any custom C# class would work,
but due to a limitation in how Unity serializes information, only basic types
like strings, ints, floats, and basic arrays will work.";
        }

        public override string[] GetAssetPaths() {
            return AssetDatabase
                .FindAssets("t:GameObject t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
        }
    }
}