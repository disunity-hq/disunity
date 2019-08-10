using System.Linq;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Extensions {

    public static class QueryableExtensions {

        /// <summary>
        /// Paginate a queryable to page with page sizes
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page">The page to paginate to. Starts at 1.</param>
        /// <param name="pageSize">The size of pages to use. 0 will disable pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int pageSize) where T : class {
            if (page > 1) {
                query = query.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0) {
                query = query.Take(pageSize);
            }

            return query;
        }

        public static IQueryable<T> AtLeast<T>(this IQueryable<T> query, VersionNumber version)
            where T : class, IVersionModel {
            if (version != null) {
                query = query.Where(m => m.VersionNumber.Major >= version.Minor)
                             .Where(m => m.VersionNumber.Minor >= version.Minor)
                             .Where(m => m.VersionNumber.Patch >= version.Patch);
            }

            return query;
        }

        public static IQueryable<T> AtMost<T>(this IQueryable<T> query, VersionNumber version)
            where T : class, IVersionModel {
            if (version != null) {
                query = query.Where(m => m.VersionNumber.Major <= version.Minor)
                             .Where(m => m.VersionNumber.Minor <= version.Minor)
                             .Where(m => m.VersionNumber.Patch <= version.Patch);
            }

            return query;
        }

        public static IQueryable<T> FindExactVersion<T>(this IQueryable<T> query, string versionNumber)
            where T : class, IVersionModel {
            return FindExactVersion(query, VersionNumber.Create(versionNumber));
        }

        public static IQueryable<T> FindExactVersion<T>(this IQueryable<T> query, VersionNumber versionNumber)
            where T : class, IVersionModel {
            return query.Include(v => v.VersionNumber)
                        .Where(v => v.VersionNumber.Major == versionNumber.Major &&
                                    v.VersionNumber.Minor == versionNumber.Minor &&
                                    v.VersionNumber.Patch == versionNumber.Patch);
        }

        public static IOrderedQueryable<T> OrderByVersion<T>(this IQueryable<T> query) where T : class, IVersionModel {
            return query.OrderBy(m => m.VersionNumber);
        }

        public static IOrderedQueryable<T> OrderByVersionDescending<T>(this IQueryable<T> query)
            where T : class, IVersionModel {
            return query.OrderByDescending(m => m.VersionNumber);
        }

    }

}