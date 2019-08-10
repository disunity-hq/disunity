using System;
using System.IO;

namespace Disunity.Store.Storage {

    public abstract class UploadStream: Stream {

        public override bool CanRead => false;

        public override bool CanSeek => false;
        
        public override long Length => throw new NotSupportedException("B2UploadStream only supports writing");

        public override long Position {
            get => throw new NotSupportedException("B2UploadStream only supports writing");
            set => throw new NotSupportedException("B2UploadStream only supports writing");
        }
        
        public override int Read(byte[] buffer, int offset, int count) {
            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public override void SetLength(long value) {

            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public abstract StorageFile FinalizeUpload();

    }

}