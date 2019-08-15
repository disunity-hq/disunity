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
        public static string CalculateManagedPath(Target target, int? hashLength=null) {
            using (var hash = MD5.Create()) {
                var pathHash = hash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(target.ExecutablePath));
                var sb = new StringBuilder();
                sb.Append(target.Slug);
                sb.Append('_');
                
                var hashChars = from b in pathHash select b.ToString("X2");
                if (hashLength != null) {
                    hashChars = hashChars.Take(hashLength.Value);
                }
                
                sb.Append(string.Join("",hashChars));
                
                return sb.ToString();
            }

        }

    }

}