using UnityEngine;


namespace Disunity.Editor.Pickers {
    public class AssetPathFilterAttribute : PropertyAttribute {
        public string[] Filters;

        public AssetPathFilterAttribute(params string[] filters) {
            Filters = filters;
        }
    }
}