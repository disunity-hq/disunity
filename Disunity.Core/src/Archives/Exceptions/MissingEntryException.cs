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

    public class MissingEntryException : Exception {

        private readonly IArchive _archive;
        private readonly EntryType _entryType;
        private readonly string _path;

        public MissingEntryException(IArchive archive, EntryType entryType, string path) {
            _archive = archive;
            _entryType = entryType;
            _path = path;

        }

    }

}