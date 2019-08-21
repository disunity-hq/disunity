using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;


namespace Disunity.Management.Util {

    public interface IZipUtil {

        Task ExtractOnlineZip( string url, string path);

    }

    public class ZipUtil : IZipUtil {

        private readonly HttpClient _client;
        private readonly IFileSystem _fileSystem;
        
        public ZipUtil(IFileSystem fileSystem, HttpClient client) {
            _fileSystem = fileSystem;
            _client = client;
        }

        public async Task ExtractOnlineZip( string url, string path) {
            var responseStream = await _client.GetStreamAsync(url);
            var zipArchive = new ZipArchive(responseStream);
            _fileSystem.Directory.CreateDirectory(path);

            foreach (var entry in zipArchive.Entries) {
                var destinationPath = _fileSystem.Path.Combine(path, entry.FullName);
                using (var destination = _fileSystem.File.Open(destinationPath, FileMode.CreateNew))
                using (var stream = entry.Open()) {
                    stream.CopyTo(destination);
                }
            }
        }

    }

}