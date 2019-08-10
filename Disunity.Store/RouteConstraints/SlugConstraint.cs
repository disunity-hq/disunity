using System.Text.RegularExpressions;

using Disunity.Core.Archives;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Disunity.Store.RouteConstraints {

    [RouteConstraint("slug")]
    public class SlugConstraint : IRouteConstraint {

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
                          RouteDirection routeDirection) {
            var value = values[routeKey]?.ToString();

            return value != null && new Regex(Schema.SLUG_PATTERN).IsMatch(value);

        }

    }

}