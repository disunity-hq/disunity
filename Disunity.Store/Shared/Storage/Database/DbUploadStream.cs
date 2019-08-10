using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Disunity.Store.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Newtonsoft.Json;

using Npgsql;


namespace Disunity.Store.Storage.Database {

    public class DbUploadStream : UploadStream {

        private readonly DbContext _context;
        private readonly string _filename;
        private readonly Dictionary<string, string> _fileInfo;
        private readonly Stream _stream;
        private readonly IDbContextTransaction _transaction;
        private readonly uint _oid;
        private readonly NpgsqlConnection _connection;

        private StorageFile _storageFile;

        public DbUploadStream(DbContext context, string filename, Dictionary<string, string> fileInfo) {
            _context = context;
            _filename = filename;
            _fileInfo = fileInfo;

            _transaction = _context.Database.BeginTransaction();

            _connection = _context.Database.GetDbConnection() as NpgsqlConnection;
            var objectManager = new NpgsqlLargeObjectManager(_connection);
            _oid = objectManager.Create();
            _stream = objectManager.OpenReadWrite(_oid);
        }

        public override bool CanWrite => _stream.CanWrite;

        public override void Flush() {
            _stream.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            _stream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value) {
            _stream.WriteByte(value);
        }

        public override StorageFile FinalizeUpload() {
            if (_storageFile != null) {
                return _storageFile;
            }

            _stream.Dispose();
            _transaction.Commit();

            var storedFile = new StoredFile {
                Id = Guid.NewGuid(),
                FileName = _filename,
                ObjectId = _oid,
                FileInfo = JsonConvert.SerializeObject(_fileInfo)
            };

            _context.Add(storedFile);
            _context.SaveChanges();

            _storageFile = new StorageFile() {
                Metadata = _fileInfo,
                FileName = _filename,
                FileId = storedFile.Id.ToString()
            };

            return _storageFile;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (!disposing) {
                return;
            }

            if (_connection.State == ConnectionState.Closed) {
                _connection.Open();
            }

            FinalizeUpload();

            _transaction.Dispose();
            _connection.Close();
        }

    }

}