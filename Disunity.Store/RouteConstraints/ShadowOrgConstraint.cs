using System.Linq;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.RouteConstraints {

    [RouteConstraint("user")]
    public class ShadowOrgConstraint : IRouteConstraint {

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
                          RouteDirection routeDirection) {
            var dbContext = httpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            var orgSlug = values[routeKey] as string;

            var user = dbContext.Users.SingleOrDefault(u => u.ShadowOrg.Slug == orgSlug);
            
            return user != null;
        }

    }

}