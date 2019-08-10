using B2Net;
using B2Net.Models;


namespace Disunity.Store.Storage.Backblaze {

    public static class B2StorageFile {

        public static StorageFile Create(B2File file) {
            if (file == null) {
                return null;
            }

            return new StorageFile() {
                FileId = file.FileId,
                FileName = file.FileName,
                Metadata = file.FileInfo
            };
        }

    }

}