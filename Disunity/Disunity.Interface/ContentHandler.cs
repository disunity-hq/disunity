using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Disunity.Interface {

    /// <summary>
    ///     Handles a Mod's content.
    /// </summary>
    public class ContentHandler {

        private readonly List<GameObject> _gameObjects;

        /// <summary>
        ///     Initialize a new ContentHandler with a Mod, ModScenes and prefabs.
        /// </summary>
        /// <param name="mod">A Mod resource</param>
        /// <param name="modScenes">ModScene resources</param>
        /// <param name="prefabs">prefab GameObjects</param>
        public ContentHandler(IResource mod, ReadOnlyCollection<IResource> modScenes,
            ReadOnlyCollection<GameObject> prefabs) {
            Mod = mod;
            ModScenes = modScenes;
            Prefabs = prefabs;

            _gameObjects = new List<GameObject>();
        }

        /// <summary>
        ///     The Mod resource.
        /// </summary>
        public IResource Mod { get; }

        /// <summary>
        ///     The Mod's ModScene resources.
        /// </summary>
        public ReadOnlyCollection<IResource> ModScenes { get; }

        /// <summary>
        ///     The Mod's prefabs.
        /// </summary>
        public ReadOnlyCollection<GameObject> Prefabs { get; }

        /// <summary>
        ///     Add a Component to a GameObject.
        /// </summary>
        /// <typeparam name="T">The Component Type.</typeparam>
        /// <param name="gameObject">The GameObject to which to add the Component.</param>
        /// <returns>The added Component.</returns>
        public T AddComponent<T>(GameObject gameObject) where T : Component {
            var component = gameObject.AddComponent<T>();

            InitializeComponent(component);

            return component;
        }

        /// <summary>
        ///     Add a Component to a GameObject.
        /// </summary>
        /// <param name="componentType">The Component Type.</param>
        /// <param name="gameObject">The GameObject to which to add the Component.</param>
        /// <returns>The added Component.</returns>
        public Component AddComponent(Type componentType, GameObject gameObject) {
            var component = gameObject.AddComponent(componentType);

            InitializeComponent(component);

            return component;
        }

        private void InitializeComponent(Component component) {
            if (component is IModHandler modHandler) {
                modHandler.OnLoaded(this);
            }
        }

        private void InitializeGameObject(GameObject go) {
            var components = go.GetComponentsInChildren<Component>();

            foreach (var component in components) {
                InitializeComponent(component);
            }
        }

        private void InitializeObject(Object obj) {
            switch (obj) {
                case GameObject gameObject:
                    _gameObjects.Add(gameObject);
                    InitializeGameObject(gameObject);
                    break;
                case Component component:
                    _gameObjects.Add(component.gameObject);
                    InitializeGameObject(component.gameObject);
                    break;
            }
        }

        /// <summary>
        ///     Create a copy of the Object original.
        /// </summary>
        /// <typeparam name="T">The Object's Type.</typeparam>
        /// <param name="original">An existing Object to copy.</param>
        /// <returns>The new Object.</returns>
        public T Instantiate<T>(T original) where T : Object {
            var obj = Object.Instantiate(original);

            InitializeObject(obj);

            return obj;
        }

        /// <summary>
        ///     Create a copy of the Object original.
        /// </summary>
        /// <param name="original">An existing Object to copy.</param>
        /// <param name="position">The position for the new Object.</param>
        /// <param name="rotation">The roration for the new Object.</param>
        /// <returns>The new Object.</returns>
        public Object Instantiate(Object original, Vector3 position, Quaternion rotation) {
            var obj = Object.Instantiate(original, position, rotation);

            InitializeObject(obj);

            return obj;
        }

        /// <summary>
        ///     Create a copy of the Object original.
        /// </summary>
        /// <param name="original">An existing Object to copy.</param>
        /// <returns>The new Object.</returns>
        public Object Instantiate(Object original) {
            return Instantiate(original, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        ///     Destroy an Object.
        /// </summary>
        /// <param name="obj">The Object to destroy.</param>
        public void Destroy(Object obj) {
            if (obj == null) {
                return;
            }

            if (obj is GameObject) {
                var gameObject = obj as GameObject;

                if (_gameObjects.Contains(gameObject)) {
                    _gameObjects.Remove(gameObject);
                }
            }

            Object.Destroy(obj);
        }

        /// <summary>
        ///     Destroy all instantiated GameObjects.
        /// </summary>
        public void Clear() {
            foreach (var gameObject in _gameObjects) {
                if (gameObject != null) {
                    Object.Destroy(gameObject);
                }
            }

            _gameObjects.Clear();
        }

    }

}