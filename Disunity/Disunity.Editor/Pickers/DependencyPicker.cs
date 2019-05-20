namespace Disunity.Editor.Pickers {

    public class DependencyEntry : HierarchyEntry {
        public StoreClient.Dependency Value => value as StoreClient.Dependency;
    }

    public class OwnerEntry : DependencyEntry {
        public override string AsString => Value.owner;
    }

    public class ModEntry : DependencyEntry {
        public override string AsString => Value.name;
    }

    public class VersionEntry : HierarchyEntry {
        public StoreClient.DependencyVersion Value => value as StoreClient.DependencyVersion;
        public override string AsString => Value.version_number;
    }

    class DependencyPicker : HierarchyPicker {

        public static DependencyPicker Create() {
            return GetWindow<DependencyPicker>();
        }



        //public static DependencyPicker Show(string current, Func<List<Dependency>> generator, EventHandler<VersionEntry> callback, FilterSet filters = null) {
        //    var w = Create();
        //    var entries = generator();
        //    var deps = GenerateGraph(entries);
        //    w.Set(new List<IEntry>(deps));
        //    w.OnSelection += (s, o) => callback(s, o as VersionEntry);
        //    Debug.Log($"Filters is null: {filters == null}");
        //    if (filters != null) {
        //        filters.Insert(0, w.SearchFilter);
        //        w.Filters = filters;
        //    }

        //    return w;
        //}

        //public static DependencyPicker DependencyField(DependencyVersion current, Func<List<Dependency>> entries, EventHandler<VersionEntry> callback, bool closeOnClick = false, FilterSet filters = null) {
        //    GUIStyle _style = new GUIStyle("TextField") {
        //        fixedHeight = 16,
        //    };

        //    if (GUILayout.Button(current == null ? "None selected." : current.full_name, _style)) {
        //        return Show("", entries, callback, filters);
        //    }

        //    return null;
        //}

    }

}
