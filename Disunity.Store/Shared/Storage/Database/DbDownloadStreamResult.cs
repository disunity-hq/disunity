using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


namespace Disunity.Store.Storage.Database {

    public class DbDownloadStreamResult : FileStreamResult {

        private readonly DbTransaction _transaction;


        public DbDownloadStreamResult(Stream fileStream, string contentType, DbTransaction transaction) : base(
            fileStream, contentType) {
            _transaction = transaction;
        }

        public DbDownloadStreamResult(Stream fileStream, MediaTypeHeaderValue contentType, DbTransaction transaction) :
            base(fileStream, contentType) {
            _transaction = transaction;
        }

        public override async Task ExecuteResultAsync(ActionContext context) {
            await base.ExecuteResultAsync(context);
            
            _transaction.Commit();
            _transaction.Connection.Close();
            _transaction.Dispose();
            FileStream.Dispose();
        }

    }

}