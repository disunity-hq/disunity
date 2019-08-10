using System;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Core.Archives;
using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Exceptions;
using Disunity.Store.Extensions;
using Disunity.Store.Pages.Mods;
using Disunity.Store.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Areas.API.v1.Mods {

    [Authorize]
    [ApiController]
    public class UploadController : ControllerBase {

        [HttpPost("api/v{version:apiVersion}/mods/upload")]
        public async Task<IActionResult> PostAsync([FromServices] ILogger<Upload> logger,
                                                   [FromServices] UserManager<UserIdentity> userManager,
                                                   [FromServices] ApplicationDbContext context,
                                                   [FromServices] Func<IFormFile, ZipArchive> archiveFactory,
                                                   [FromServices] Func<ZipArchive, Task<ModVersion>> modVersionFactory,
                                                   [FromServices] IStorageProvider storage,
                                                   IFormFile ArchiveUpload) {

            try {
                var archive = archiveFactory(ArchiveUpload);
                var manifest = archive.GetManifest();

                var user = await userManager.GetUserAsync(HttpContext.User);

                if (user == null) {
                    return Unauthorized();
                }

                var uploadedFile = await storage.UploadArchive(archive);

                var modVersion = await modVersionFactory(archive);
                modVersion.FileId = uploadedFile.FileId;

                await context.SaveChangesAsync();

                return new JsonResult(new {manifest.DisplayName});
            }
            catch (ApiException e) {
                return e.Error;
            }
            catch (AggregateException e) {
                return e.InnerExceptions
                        .OfType<ApiException>()
                        .Select(exc => exc.Error).AsAggregate();
            }
        }

    }

}