using Disunity.Core;

using UnityEngine.SceneManagement;


namespace Disunity.Runtime {

    /// <summary>
    ///     Represents a Scene that is included in a Mod.
    /// </summary>
    public class ModScene {

        /// <summary>
        ///     Initialize a new ModScene with a Scene name and a Mod
        /// </summary>
        /// <param name="name">The scene's name</param>
        /// <param name="mod">The Mod this ModScene belongs to.</param>
        public ModScene(string name, Mod mod) {
            Name = name;
            Mod = mod;
            Scene = null;
        }

        public string Name { get; private set; }

        /// <summary>
        ///     This ModScene's Scene.
        /// </summary>
        public Scene? Scene { get; private set; }

        /// <summary>
        ///     The Mod this scene belongs to.
        /// </summary>
        public Mod Mod { get; }

        protected void Load() {
            SceneManager.LoadScene(Name, LoadSceneMode.Additive);
            Scene = SceneManager.GetSceneByName(Name);
            SetActive();
        }

        /// <summary>
        ///     Set this ModScene's Scene as the active scene.
        /// </summary>
        public void SetActive() {
            if (Scene.HasValue) {
                SceneManager.SetActiveScene(Scene.Value);
            }
        }

        /// <summary>
        ///     Returns the first Component of type T in this Scene.
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T GetComponentInScene<T>() {
            return Scene.Value.GetComponentInScene<T>();
        }

        /// <summary>
        ///     Returns all Components of type T in this Scene.
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T[] GetComponentsInScene<T>() {
            return Scene.Value.GetComponentsInScene<T>();
        }

    }

}