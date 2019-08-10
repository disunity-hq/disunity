using System;
using System.IO;
using System.Linq;
using System.Net;

using Disunity.Core.Archives;
using Disunity.Store.Errors;

using Microsoft.AspNetCore.Http;


namespace Disunity.Store.Util {

    public static class ArchiveFileValidator {

        private static string GetFileName(IFormFile formFile) {
            return WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
        }

        private static void CheckMimeType(IFormFile formFile, string fileName, params string[] mimeTypes) {

            var fileMimeType = formFile.ContentType.ToLower();

            if (!mimeTypes.Contains(fileMimeType)) {
                var msg =
                    $"The file {fileName} must be of type {string.Join(", or ", mimeTypes.Select(s => $"`{s}`"))}. Type `{fileMimeType}` was provided";

                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        private static void CheckEmpty(IFormFile formFile, string fileName) {
            if (formFile.Length == 0) {
                var msg = $"The file {fileName} is empty.";
                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        private static void CheckSize(IFormFile formFile, string fileName) {
            if (formFile.Length > 1048576) {
                var msg = $"The file {fileName} exceeds 1 MB.";
                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        public static void Validate(IFormFile formFile) {
            var fileName = GetFileName(formFile);
            CheckMimeType(formFile, fileName, "application/zip", "application/x-zip", "application/x-zip-compressed");
            CheckEmpty(formFile, fileName);
            CheckSize(formFile, fileName);
        }        

    }

}