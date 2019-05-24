using System.IO;
using UnityEngine;


namespace Disunity.Editor {

    public class AsmDef {

        public bool allowUnsafeCode;
        public bool autoReferenced;
        public string[] defineConstraints;
        public string name;
        public string[] optionalUnityReferences;
        public bool overrideReferences;
        public string[] precompiledReferences;
        public string[] references;


        public static AsmDef FromAssetPath(string path) {
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<AsmDef>(json);
        }

    }

}