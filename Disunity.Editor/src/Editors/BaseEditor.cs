using Disunity.Editor.Windows;


namespace Disunity.Editor.Editors {

    internal abstract class BaseEditor {

        protected bool _initialized;

        protected ExporterWindow _window;

        protected BaseEditor(ExporterWindow window) {
            _window = window;
            _initialized = false;
        }

        public abstract string Label();
        public abstract string Title();
        public abstract string Help();
        public abstract void Draw();
        public virtual void Init() { }

        public virtual void OnGUI() {
            if (!_initialized) {
                Init();
                _initialized = true;
            }

            Draw();
        }

    }

}