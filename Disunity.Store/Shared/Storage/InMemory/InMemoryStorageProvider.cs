using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Storage.InMemory {

    public class InMemoryStorageProvider : IStorageProvider {

        private class FileInfo {

            public byte[] Data { get; set; }

            public string Name { get; set; }

            public Dictionary<string, string> MetaData { get; set; }

        }

        private readonly Dictionary<Guid, FileInfo> _files = new Dictionary<Guid, FileInfo>();

        public Task<StorageFile> UploadFile(byte[] fileData, string filename,
                                            Dictionary<string, string> fileInfo = null) {
            var fileId = Guid.NewGuid();
            var file = new StorageFile() {Metadata = fileInfo, FileId = fileId.ToString(), FileName = filename};
            _files.Add(fileId, new FileInfo() {Data = fileData, Name = filename, MetaData = fileInfo});

            return Task.FromResult(file);
        }

        public async Task<StorageFile> UploadFile(Stream stream, string filename,
                                                  Dictionary<string, string> fileInfo = null) {
            using (var memoryStream = new MemoryStream()) {
                await stream.CopyToAsync(memoryStream);
                return await UploadFile(memoryStream.ToArray(), filename, fileInfo);
            }
        }

        public UploadStream GetUploadStream(string filename, Dictionary<string, string> fileInfo = null) {
            return new InMemoryUploadStream(this, filename, fileInfo);
        }

        public Task<IActionResult> GetDownloadAction(string fileId) {
            var fileGuid = Guid.Parse(fileId);
            var file = _files.GetValueOrDefault(fileGuid);

            IActionResult result;

            if (file == null) {
                result = new NotFoundResult();
            } else {
                result = new FileContentResult(file.Data, "application/zip") {
                    FileDownloadName = file.Name
                };
            }

            return Task.FromResult(result);
        }

        public Task DeleteFile(string fileId) {
            var guid = Guid.Parse(fileId);

            if (_files.ContainsKey(guid)) {
                _files.Remove(guid);
            }

            return Task.CompletedTask;
        }

    }

}