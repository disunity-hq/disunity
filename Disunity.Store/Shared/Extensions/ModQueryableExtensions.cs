using System;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Errors;
using Disunity.Store.Exceptions;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Extensions {

    public static class ModQueryableExtensions {

        public static Task<Mod> FindModByDepString(this IQueryable<Mod> mods,
                                                   string depString) {
            var segments = depString.Split('/');
            var orgSlug = segments[0];
            var modSlug = segments[1];


            var mod = mods
                      .Where(m => m.Slug == modSlug)
                      .SingleOrDefaultAsync(m => m.Owner.Slug == orgSlug);

            if (mod != null) return mod;
            
            throw new ModNotFoundError($"Could not find mod matching dependency string {depString}").ToExec();
        }

    }

}