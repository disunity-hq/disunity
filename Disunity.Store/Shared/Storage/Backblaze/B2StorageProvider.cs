using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using B2Net;
using B2Net.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Storage.Backblaze {

    public class B2StorageProvider : IStorageProvider {

        private readonly ILogger<B2StorageProvider> _logger;

        private readonly B2Client _client;
        private readonly B2Options _b2Options;
        private readonly string _bucketName;

        public bool ServiceConfigured => _client != null;

        public B2StorageProvider(IConfiguration config, ILogger<B2StorageProvider> logger) {
            _b2Options = new B2Options() {
                AccountId = config["Auth:B2:AccountId"],
                KeyId = config["Auth:B2:KeyId"],
                ApplicationKey = config["Auth:B2:AppKey"],
                BucketId = config["Auth:B2:BucketId"],
                PersistBucket = true
            };

            // if backblaze isn't fully configured
            if (string.IsNullOrEmpty(_b2Options.AccountId) ||
                string.IsNullOrEmpty(_b2Options.KeyId) ||
                string.IsNullOrEmpty(_b2Options.ApplicationKey) ||
                string.IsNullOrEmpty(_b2Options.BucketId)) {
                throw new InvalidOperationException("Backblaze not fully configured.");

            }

            _client = new B2Client(B2Client.Authorize(_b2Options));
            _logger = logger;

            _bucketName = _client.Buckets.GetList().Result
                                 .Single(b => b.BucketId == _b2Options.BucketId)
                                 .BucketName;
        }

        public async Task<StorageFile> UploadFile(byte[] fileData, string filename,
                                                  Dictionary<string, string> fileInfo = null) {
            if (!ServiceConfigured) {
                _logger.LogWarning("Attempting to upload file when b2 is not configured");
                return null;
            }

            var file = await _client.Files.Upload(fileData, filename, "", fileInfo);

            return B2StorageFile.Create(file);
        }

        public async Task<StorageFile> UploadFile(Stream stream, string filename,
                                                  Dictionary<string, string> fileInfo = null) {
            using (var uploadStream = GetUploadStream(filename, fileInfo)) {
                await stream.CopyToAsync(uploadStream);
                return uploadStream.FinalizeUpload();
            }
        }

        public UploadStream GetUploadStream(string filename, Dictionary<string, string> fileInfo = null) {
            if (!ServiceConfigured) {
                _logger.LogWarning("Attempting to upload file when b2 is not configured");
                return null;
            }

            var stream = new B2UploadStream(_client, filename, _b2Options.BucketId, fileInfo);
            return stream;
        }


        public Task<IActionResult> GetDownloadAction(string fileId) {
            return Task.FromResult<IActionResult>(new RedirectResult(GetDownloadUrl(fileId)));
        }

        public async Task DeleteFile(string fileId) {
            B2File fileInfo = null;

            try {
                fileInfo = await _client.Files.GetInfo(fileId);
            }
            catch {
                // ignored
            }

            if (fileInfo != null)
                await _client.Files.Delete(fileId, fileInfo.FileName);
        }

        public string GetDownloadUrl(string fileId) {
            return $"{_b2Options.DownloadUrl}/b2api/v2/b2_download_file_by_id?fileId={fileId}";
        }

    }

}