using System.Linq;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.RouteConstraints {

    [RouteConstraint("org")]
    public class OrgConstraint : IRouteConstraint {

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
                          RouteDirection routeDirection) {
            var dbContext = httpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            var orgSlug = values[routeKey] as string;
            var org = dbContext.Orgs.SingleOrDefault(o => o.Slug == orgSlug);

            if (org == null) {
                return false;
            }

            var user = dbContext.Users.SingleOrDefault(u => u.ShadowOrgId == org.Id);

            return user == null;
        }

    }

}