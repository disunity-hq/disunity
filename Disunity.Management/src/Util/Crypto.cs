using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Disunity.Management.Models;


namespace Disunity.Management.Util {

    public static class Crypto {

        /// <summary>
        /// Calculate the 
        /// </summary>
        /// <param name="managedPath"></param>
        /// <param name="target"></param>
        /// <param name="hashLength"></param>
        /// <returns></returns>
        public static string CalculateManagedPath(Target target, int? hashLength = null) {
            using (var hash = MD5.Create()) {
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

        public static string HashString(string data, HashAlgorithm algorithm = null) {
            var cleanup = false;

            if (algorithm == null) {
                algorithm = MD5.Create();
                cleanup = true;
            }

            var sb = new StringBuilder();
            var bytes = algorithm.ComputeHash(Encoding.ASCII.GetBytes(data));

            foreach (var letter in bytes) {
                sb.Append(letter.ToString("X2"));
            }

            if (cleanup) {
                algorithm.Dispose();
            }

            return sb.ToString();
        }

    }

}