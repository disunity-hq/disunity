using System;
using System.IO;

using BindingAttributes;

using Disunity.Core.Archives;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


namespace Disunity.Store.Util {

    public class CoreFactories {

        [Factory]
        public static Func<string, Manifest> ManifestFactory(IServiceProvider sp) {
            return json => {
                Manifest.ValidateJson(json);
                var manifest = JsonConvert.DeserializeObject<Manifest>(json);
                return manifest;
            };
        }

        [Factory]
        public static Func<Stream, ZipArchive> StreamArchiveFactory(IServiceProvider services) {
            var manifestFactory = services.GetRequiredService<Func<string, Manifest>>();
            return stream => new ZipArchive(manifestFactory, stream);
        }

        [Factory]
        public static Func<IFormFile, ZipArchive> ArchiveFactory(IServiceProvider services) {
            var log = services.GetRequiredService<ILogger<CoreFactories>>();
            var archiveFactory = services.GetRequiredService<Func<Stream, ZipArchive>>();
            var apiArchiveValidator = services.GetRequiredService<ApiArchiveValidator>();

            return formFile => {
                ArchiveFileValidator.Validate(formFile);
                var archive = archiveFactory(formFile.OpenReadStream());
                apiArchiveValidator.Validate(archive);
                return archive;
            };
        }

    }

}