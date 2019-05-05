using System.Collections;
using Disunity.Interface;
using UnityEngine.SceneManagement;


namespace Disunity.Core {

    /// <summary>
    ///     Represents a Scene that is included in a Mod.
    /// </summary>
    public class ModScene : Resource {

        /// <summary>
        ///     Initialize a new ModScene with a Scene name and a Mod
        /// </summary>
        /// <param name="name">The scene's name</param>
        /// <param name="mod">The Mod this ModScene belongs to.</param>
        public ModScene(string name, Mod mod) : base(name) {
            Mod = mod;
            Scene = null;
        }

        /// <summary>
        ///     This ModScene's Scene.
        /// </summary>
        public Scene? Scene { get; private set; }

        /// <summary>
        ///     The Mod this scene belongs to.
        /// </summary>
        public Mod Mod { get; }

        /// <summary>
        ///     Can the scene be loaded? False if this scene's Mod is not loaded.
        /// </summary>
        public override bool CanLoad => Mod.LoadState == ResourceLoadState.Loaded;

        protected override IEnumerator LoadResources() {
            //NOTE: Loading a scene synchronously prevents the scene from being initialized, so force async loading.
            yield return LoadResourcesAsync();
        }

        protected override IEnumerator LoadResourcesAsync() {
            var loadOperation = SceneManager.LoadSceneAsync(Name, LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;

            while (loadOperation.progress < .9f) {
                LoadProgress = loadOperation.progress;
                yield return null;
            }

            loadOperation.allowSceneActivation = true;

            yield return loadOperation;

            Scene = SceneManager.GetSceneByName(Name);

            SetActive();
        }

        protected override void UnloadResources() {
            Scene?.Unload();
            Scene = null;
        }

        /// <summary>
        ///     Set this ModScene's Scene as the active scene.
        /// </summary>
        public void SetActive() {
            if (Scene.HasValue) {
                SceneManager.SetActiveScene(Scene.Value);
            }
        }

        protected override void OnLoaded() {
            foreach (var modHandler in GetComponentsInScene<IModHandler>()) {
                modHandler.OnLoaded(Mod.ContentHandler);
            }

            base.OnLoaded();
        }

        /// <summary>
        ///     Returns the first Component of type T in this Scene.
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T GetComponentInScene<T>() {
            return LoadState != ResourceLoadState.Loaded ? default : Scene.Value.GetComponentInScene<T>();
        }

        /// <summary>
        ///     Returns all Components of type T in this Scene.
        /// </summary>
        /// <typeparam name="T">The Component that will be looked for.</typeparam>
        /// <returns>An array of found Components of Type T.</returns>
        public T[] GetComponentsInScene<T>() {
            return LoadState != ResourceLoadState.Loaded ? new T[0] : Scene.Value.GetComponentsInScene<T>();
        }
    }
}