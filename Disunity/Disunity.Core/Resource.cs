using System;
using System.Collections;
using Disunity.Interface;


namespace Disunity.Core {

    /// <summary>
    ///     A class that supports async loading of various resources.
    /// </summary>
    public abstract class Resource : IResource {

        private float _loadProgress;
        private BaseLoadState _baseLoadState;

        /// <summary>
        ///     Initialize a Resource with a name.
        /// </summary>
        /// <param name="name">The Resource's name</param>
        protected Resource(string name) {
            Name = name;
            _baseLoadState = new UnloadedState(this);
        }

        /// <summary>
        ///     Is this Resource busy loading?
        /// </summary>
        public virtual bool IsBusy => _baseLoadState.IsBusy;

        /// <summary>
        ///     Can this Resource be loaded?
        /// </summary>
        public virtual bool CanLoad => true;

        /// <summary>
        ///     This Resource's current load state.
        /// </summary>
        public ResourceLoadState LoadState => _baseLoadState.LoadState;

        /// <summary>
        ///     What is the Resource's load progress.
        /// </summary>
        public float LoadProgress {
            get => _loadProgress;
            protected set {
                if (value == _loadProgress) {
                    return;
                }

                _loadProgress = value;
                LoadProgression?.Invoke(_loadProgress);
            }
        }

        /// <summary>
        ///     This Resource's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Load this Resource.
        /// </summary>
        public void Load() {
            Dispatcher.Instance.Enqueue(LoadCoroutine());
        }

        /// <summary>
        ///     Load this Resource asynchronously.
        /// </summary>
        public void LoadAsync() {
            Dispatcher.Instance.Enqueue(LoadAsyncCoroutine());
        }

        /// <summary>
        ///     Unload this Resource.
        /// </summary>
        public void Unload() {
            _baseLoadState.Unload();
        }

        /// <summary>
        ///     Occurs when this Resource has been loaded.
        /// </summary>
        public event Action<Resource> Loaded;

        /// <summary>
        ///     Occurs when this Resource has been unloaded.
        /// </summary>
        public event Action<Resource> Unloaded;

        /// <summary>
        ///     Occurs when this Resource's async loading has been cancelled.
        /// </summary>
        public event Action<Resource> LoadCancelled;

        /// <summary>
        ///     Occurs when this Resources async loading has been resumed.
        /// </summary>
        public event Action<Resource> LoadResumed;

        /// <summary>
        ///     Occurs when this Resource's loadProgress changes.
        /// </summary>
        public event Action<float> LoadProgression;

        /// <summary>
        ///     Coroutine that loads this Resource.
        /// </summary>
        public IEnumerator LoadCoroutine() {
            yield return _baseLoadState.Load();
        }

        /// <summary>
        ///     Coroutine that loads this Resource asynchronously.
        /// </summary>
        public IEnumerator LoadAsyncCoroutine() {
            yield return _baseLoadState.LoadAsync();
        }

        /// <summary>
        ///     Finalize the current BaseLoadState.
        /// </summary>
        protected void End() {
            _baseLoadState.End();
        }

        /// <summary>
        ///     Use this to implement anything that should happen before unloading this Resource.
        /// </summary>
        protected virtual void PreUnLoadResources() {
        }

        /// <summary>
        ///     Use this to implement unloading this Resource.
        /// </summary>
        protected abstract void UnloadResources();

        /// <summary>
        ///     Use this to implement loading this Resource.
        /// </summary>
        protected abstract IEnumerator LoadResources();

        /// <summary>
        ///     Use this to implement loading this Resource asynchronously.
        /// </summary>
        protected abstract IEnumerator LoadResourcesAsync();

        /// <summary>
        ///     Handle end of loading.
        /// </summary>
        protected virtual void OnLoaded() {
            LoadProgress = 1;
            Loaded?.Invoke(this);
        }

        /// <summary>
        ///     Handle end of unloading.
        /// </summary>
        protected virtual void OnUnloaded() {
            _loadProgress = 0;
            Unloaded?.Invoke(this);
        }

        /// <summary>
        ///     Handle load cancelling.
        /// </summary>
        protected virtual void OnLoadCancelled() {
            LoadCancelled?.Invoke(this);
        }

        /// <summary>
        ///     Handle load resuming.
        /// </summary>
        protected virtual void OnLoadResumed() {
            LoadResumed?.Invoke(this);
        }

        private abstract class BaseLoadState {

            protected readonly Resource Resource;

            protected BaseLoadState(Resource resource) {
                Resource = resource;
            }

            public virtual bool IsBusy => false;

            public abstract ResourceLoadState LoadState { get; }

            public virtual IEnumerator Load() {
                yield break;
            }

            public virtual IEnumerator LoadAsync() {
                yield break;
            }

            public virtual void Unload() {
            }

            public virtual void End() {
            }

        }

        private class UnloadedState : BaseLoadState {

            public UnloadedState(Resource resource) : base(resource) {
            }

            public override ResourceLoadState LoadState => ResourceLoadState.Unloaded;

            public override IEnumerator Load() {
                if (Resource.CanLoad) {
                    Resource._baseLoadState = new LoadingState(Resource);
                    yield return Resource.LoadResources(); //TODO: this skips a frame
                    Resource.End();
                }
            }

            public override IEnumerator LoadAsync() {
                if (Resource.CanLoad) {
                    Resource._baseLoadState = new LoadingState(Resource);
                    yield return Resource.LoadResourcesAsync();
                    Resource.End();
                }
            }

        }

        private class LoadingState : BaseLoadState {

            public LoadingState(Resource resource) : base(resource) {
            }

            public override bool IsBusy => true;

            public override ResourceLoadState LoadState => ResourceLoadState.Loading;

            public override void End() {
                Resource._baseLoadState = new LoadedState(Resource);
                Resource.OnLoaded();
            }

            public override void Unload() {
                Resource._baseLoadState = new CancellingState(Resource);
            }

        }

        private class LoadedState : BaseLoadState {

            public LoadedState(Resource resource) : base(resource) {
            }

            public override ResourceLoadState LoadState => ResourceLoadState.Loaded;

            public override void Unload() {
                if (Resource.IsBusy) {
                    Resource.PreUnLoadResources();
                    Resource._baseLoadState = new UnloadingState(Resource);
                }
                else {
                    Resource.PreUnLoadResources();
                    Resource.UnloadResources();
                    Resource._baseLoadState = new UnloadedState(Resource);
                    Resource.OnUnloaded();
                }
            }

        }

        private class CancellingState : BaseLoadState {

            public CancellingState(Resource resource) : base(resource) {
            }

            public override bool IsBusy => true;

            public override ResourceLoadState LoadState => ResourceLoadState.Cancelling;

            public override IEnumerator Load() {
                Resource.OnLoadResumed();
                Resource._baseLoadState = new LoadingState(Resource);
                yield break;
            }

            public override IEnumerator LoadAsync() {
                Resource.OnLoadResumed();
                Resource._baseLoadState = new LoadingState(Resource);
                yield break;
            }

            public override void End() {
                Resource._baseLoadState = new UnloadedState(Resource);
                Resource.PreUnLoadResources();
                Resource.UnloadResources();
                Resource.OnLoadCancelled();
            }

        }

        private class UnloadingState : BaseLoadState {

            public UnloadingState(Resource resource) : base(resource) {
            }

            public override bool IsBusy => true;

            public override ResourceLoadState LoadState => ResourceLoadState.Unloading;

            public override IEnumerator Load() {
                Resource._baseLoadState = new LoadedState(Resource);
                Resource.OnLoaded();
                yield break;
            }

            public override IEnumerator LoadAsync() {
                Resource._baseLoadState = new LoadedState(Resource);
                Resource.OnLoaded();
                yield break;
            }

            public override void End() {
                Resource.PreUnLoadResources();
                Resource.UnloadResources();
                Resource._baseLoadState = new UnloadedState(Resource);
                Resource.OnUnloaded();
            }

        }

    }

}