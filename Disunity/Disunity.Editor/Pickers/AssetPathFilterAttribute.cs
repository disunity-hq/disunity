using UnityEngine;

namespace Disunity.Editor {
    public class AssetPathFilterAttribute : PropertyAttribute {
        public string[] Filters;

        public AssetPathFilterAttribute(params string[] filters) {
            Filters = filters;
        }
    }
}