using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Extensions;
using Disunity.Store.Storage;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Middleware {

    public class DownloadRedirectMiddleware {

        private readonly RequestDelegate _next;
        private readonly ILogger<DownloadRedirectMiddleware> _logger;

        public DownloadRedirectMiddleware(RequestDelegate next,
                                          ILogger<DownloadRedirectMiddleware> logger) {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext, IStorageProvider storage) {
            var regex = new Regex(
                @"/u/(?'owner'[a-z\d\.]+(?:-[a-z\d\.]+)*)/(?'mod'[a-z\d]+(?:-[a-z\d]+)*)/(?'version'\d+\.\d\.\d)/download");

            var match = regex.Match(context.Request.Path);

            if (match.Success) {
                var modVersion = await FindModVersion(dbContext, match);

                if (modVersion != null) {
                    await ExecuteDownload(context, storage, modVersion, dbContext);
                    return;
                }

            }

            await _next(context);
        }

        private async Task ExecuteDownload(HttpContext context, IStorageProvider storage, ModVersion modVersion,
                                           ApplicationDbContext dbContext) {
            _logger.LogInformation(
                $"Executing download request for {modVersion.DisplayName}@{modVersion.VersionNumber}");

            await CountDownload(context, modVersion, dbContext);

            var downloadAction = await storage.GetDownloadAction(modVersion.FileId);

            switch (downloadAction) {
                case RedirectResult actionResult:
                    _logger.LogDebug($"Redirecting to {actionResult.Url}");
                    await context.ExecuteResultAsync(actionResult);
                    break;

                case FileContentResult actionResult:
                    await context.ExecuteResultAsync(actionResult);
                    break;

                case FileStreamResult actionResult:
                    await context.ExecuteResultAsync(actionResult);
                    break;
            }

        }

        private static async Task CountDownload(HttpContext context, ModVersion modVersion,
                                                ApplicationDbContext dbContext) {
            var sourceIp = context.Connection.RemoteIpAddress.ToString();

            var downloadEvent =
                await dbContext.ModVersionDownloadEvents.FirstOrDefaultAsync(e => e.SourceIp == sourceIp);

            if (downloadEvent == null) {
                downloadEvent = new ModVersionDownloadEvent() {
                    ModVersion = modVersion,
                    SourceIp = sourceIp
                };

                dbContext.Add(downloadEvent);
            }


            if (downloadEvent.TryCountDownload()) {
                modVersion.Downloads++;
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task<ModVersion> FindModVersion(ApplicationDbContext dbContext, Match match) {
            var ownerSlug = match.Groups["owner"].Value;
            var modSlug = match.Groups["mod"].Value;
            var versionString = match.Groups["version"].Value;

            _logger.LogInformation($"Download request for {ownerSlug}/{modSlug}@{versionString}");

            var modVersion = await dbContext.ModVersions
                                            .Where(v => v.Mod.Slug == modSlug && v.Mod.Owner.Slug == ownerSlug)
                                            .FindExactVersion(VersionNumber.Create(versionString))
                                            .FirstOrDefaultAsync();

            return modVersion;
        }

    }

}