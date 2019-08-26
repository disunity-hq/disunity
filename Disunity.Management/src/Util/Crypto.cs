using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Management.Models;

using Microsoft.Extensions.Options;


namespace Disunity.Management.Util {

    public class CryptoOptions {

        public string Algo { get; set; } = "md5";

    }

    [AsScoped]
    public class Crypto {

        private readonly string _algo;

        public Crypto(IOptionsMonitor<CryptoOptions> optionsMonitor) {
            _algo = (optionsMonitor?.CurrentValue ?? new CryptoOptions()).Algo;
        }

        /// <summary>
        /// Calculate the managed path for a target by hashing the target's executable path
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hashLength"></param>
        /// <returns></returns>
        public string CalculateManagedPath(Target target, int? hashLength = null) {
            using (var hash = HashAlgorithm.Create(_algo)) {
                var pathHash = hash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(target.ExecutablePath));
                var sb = new StringBuilder();
                sb.Append(target.Slug);

                var hashChars = (from b in pathHash select b.ToString("X2")).ToList();

                if (hashLength != null) {
                    hashChars = hashChars.Take(hashLength.Value).ToList();
                }

                if (hashChars.Any()) {
                    sb.Append('_');
                    sb.Append(string.Join("", hashChars));
                }

                return sb.ToString();
            }

        }

        public string HashString(string data) {
            using (var hash = HashAlgorithm.Create(_algo)) {
                var sb = new StringBuilder();
                var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(data));

                foreach (var letter in bytes) {
                    sb.Append(letter.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public async Task<string> HashFile(string path, IFileSystem fileSystem, CancellationToken cancellationToken = default) {
            using (var hash = HashAlgorithm.Create(_algo)) {
                var sb = new StringBuilder();
                var bytes = new byte[0];

                using (var file = fileSystem.File.OpenRead(path)) {
                    await Task.Run(() => bytes = hash.ComputeHash(file), cancellationToken);
                }

                foreach (var letter in bytes) {
                    sb.Append(letter.ToString("X2"));
                }

                return sb.ToString();
            }
        }

    }

}