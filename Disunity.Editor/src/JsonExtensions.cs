using System;

using UnityEngine;


namespace Disunity.Editor {

    public static class JsonExtensions {

        public static T[] FromJson<T>(string json) {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            var wrapper = new Wrapper<T> {Items = array};
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) {
            var wrapper = new Wrapper<T> {Items = array};
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> {

            public T[] Items;

        }

    }

}