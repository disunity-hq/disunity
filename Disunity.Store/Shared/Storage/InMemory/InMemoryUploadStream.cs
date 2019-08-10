using System.Collections.Generic;
using System.IO;


namespace Disunity.Store.Storage.InMemory {

    public class InMemoryUploadStream : UploadStream {

        private readonly InMemoryStorageProvider _storageProvider;
        private readonly string _filename;
        private readonly Dictionary<string, string> _fileInfo;

        private readonly MemoryStream _buffer = new MemoryStream();
        private StorageFile _storedFile;

        public override bool CanWrite => _storedFile == null;

        public InMemoryUploadStream(InMemoryStorageProvider storageProvider, string filename,
                                    Dictionary<string, string> fileInfo) {
            _storageProvider = storageProvider;
            _filename = filename;
            _fileInfo = fileInfo;
        }

        public override void Flush() { }

        public override void Write(byte[] buffer, int offset, int count) {
            _buffer.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value) {
            _buffer.WriteByte(value);
        }

        public override StorageFile FinalizeUpload() {
            _buffer.Seek(0, SeekOrigin.Begin);
            return _storedFile ?? (_storedFile = _storageProvider.UploadFile(_buffer, _filename, _fileInfo).Result);

        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (!disposing) {
                return;
            }

            FinalizeUpload();
            
            _buffer.Dispose();
        }

    }

}