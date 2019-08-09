using System;


namespace Disunity.Core.Archives.Exceptions {

    public enum EntryType {

        Manifest,
        Readme,
        Artifact,
        PreloadAssembly,
        RuntimeAssembly,
        PrefabBundle,
        SceneBundle

    }

    public class MissingEntryException : BaseDisunityException {

        public IArchive Archive { get; }
        public EntryType EntryType { get; }
        public string Path { get; }

        public MissingEntryException(IArchive archive, EntryType entryType, string path) {
            Archive = archive;
            EntryType = entryType;
            Path = path;

        }

    }

}