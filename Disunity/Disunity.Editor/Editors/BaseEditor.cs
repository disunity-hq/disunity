using UnityEditor;


namespace Disunity.Editor.Editors {
    internal interface IEditor {

        string Label();
        string Title();
        void Draw();
        string Help();

    }

    public abstract class BaseEditor : IEditor {

        protected EditorWindow _window;

        public abstract string Label();
        public abstract string Title();
        public abstract string Help();
        public abstract void Draw();

        public BaseEditor(EditorWindow window) {
            _window = window;
        }
    }
}
