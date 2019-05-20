namespace Disunity.Editor.Pickers {

    public class AssetEntry : HierarchyEntry {
        public string Value => value as string;
        public string PathPart { get; set; }
        public override string AsString => PathPart;
    }

    class AssetPicker : HierarchyPicker { }

}
