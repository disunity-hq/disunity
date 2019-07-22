using UnityEngine;


namespace Disunity.Runtime {

    /// <summary>
    ///     A generic singleton class for MonoBehaviours
    /// </summary>
    /// <typeparam name="T">The singleton's Type.</typeparam>
    public class UnitySingleton<T> : MonoBehaviour where T : Component {

        private static T _instance;

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static T Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                _instance = FindObjectOfType<T>();

                if (_instance != null) {
                    return _instance;
                }

                var obj = new GameObject {name = typeof(T).Name};
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy() {
            if (_instance == this) {
                _instance = null;
            }
        }

    }

}