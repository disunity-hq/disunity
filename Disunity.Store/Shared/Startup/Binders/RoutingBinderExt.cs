using System.Linq;
using System.Reflection;

using Disunity.Store.RouteConstraints;

using Microsoft.Extensions.DependencyInjection;

using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Startup.Binders {

    public static class RoutingBinderExt {

        public static void ConfigureRouting(this IServiceCollection services) {
            var customConstraints = Assembly.GetExecutingAssembly().GetTypes()
                                            .Where(t => t.GetCustomAttribute<RouteConstraintAttribute>() != null);


            services.AddRouting(options => {
                options.LowercaseUrls = true;

                foreach (var constraint in customConstraints) {
                    var attr = constraint.GetCustomAttribute<RouteConstraintAttribute>();
                    options.ConstraintMap.Add(attr.ConstraintName, constraint);
                }
            });

            var assembly = typeof(RoutingBinderExt).Assembly;
            services.AddBreadcrumbs(assembly);
        }

    }

}