using System;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Errors;
using Disunity.Store.Exceptions;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Extensions {

    public static class ModVersionQueryableExtensions {

        public static async Task<ModVersion> FindModVersionByDepString(this IQueryable<ModVersion> modVersions,
                                                                       string depString, string versionString) {
            if (versionString == null) {
                return null;
            }

            var segments = depString.Split('/');
            var orgSlug = segments[0];
            var modSlug = segments[1];

            try {
                return await modVersions
                             .Where(v => v.Mod.Slug == modSlug)
                             .Where(v => v.Mod.Owner.Slug == orgSlug)
                             .SingleAsync(v => v.VersionNumber == versionString);
            }
            catch (InvalidOperationException e) {
                throw new ModNotFoundError($"Unable to find version matching {depString}@{versionString}").ToExec();
            }

        }

    }

}