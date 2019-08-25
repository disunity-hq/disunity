using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

using BindingAttributes;


namespace Disunity.Management.Services {

    public interface IZipUtil {

        Task<string> ExtractOnlineZip( string url, string path);

    }

    [AsSingleton(typeof(IZipUtil))]
    public class ZipUtil : IZipUtil {

        private readonly HttpClient _client;
        private readonly IFileSystem _fileSystem;
        
        public ZipUtil(IFileSystem fileSystem, HttpClient client) {
            _fileSystem = fileSystem;
            _client = client;
        }

        public async Task<string> ExtractOnlineZip( string url, string path) {
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var zipArchive = new ZipArchive(await response.Content.ReadAsStreamAsync());
            _fileSystem.Directory.CreateDirectory(path);

            foreach (var entry in zipArchive.Entries) {
                var destinationPath = _fileSystem.Path.Combine(path, entry.FullName);
                using (var destination = _fileSystem.File.Open(destinationPath, FileMode.CreateNew))
                using (var stream = entry.Open()) {
                    stream.CopyTo(destination);
                }
            }

            return path;
        }

    }

}