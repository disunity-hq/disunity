using System.Collections.Generic;
using System.Linq;


namespace Disunity.Store.Extensions {

    public static class EnumerableExtensions {

        /// <summary>
        /// Paginate a queryable to page with page sizes 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page">The page to paginate to. Starts at 1.</param>
        /// <param name="pageSize">The size of pages to use. 0 will disable pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, int page, int pageSize) where T : class {
            if (page > 1) {
                query = query.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0) {
                query = query.Take(pageSize);
            }

            return query;
        }

    }

}