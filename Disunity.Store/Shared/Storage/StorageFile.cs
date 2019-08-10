using System.Collections.Generic;


namespace Disunity.Store.Storage {

    public class StorageFile {

        public string FileName { get; set; }

        public string FileId { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

    }

}