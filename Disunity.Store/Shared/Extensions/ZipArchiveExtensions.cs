using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Core.Archives;


namespace Disunity.Store.Extensions {

    public static class ZipArchiveExtensions {

        public static void CopyTo(this ZipArchive archive, Stream stream) {
            archive.Stream.Seek(0, SeekOrigin.Begin);
            archive.Stream.CopyTo(stream);
        }

        public static Task CopyToAsync(this ZipArchive archive, Stream stream, CancellationToken cancellationToken = default(CancellationToken)) {
            archive.Stream.Seek(0, SeekOrigin.Begin);
            return archive.Stream.CopyToAsync(stream, cancellationToken);
        }

    }

}