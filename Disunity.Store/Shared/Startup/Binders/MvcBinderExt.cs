using Disunity.Store.Policies;
using Disunity.Store.Startup.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Swashbuckle.AspNetCore.Swagger;


namespace Disunity.Store.Startup.Binders {

    public static class MvcBinderExt {

        public static void ConfigureMvc(this IServiceCollection services) {
            services.AddMvc(options => {
                        options.Filters.Add(new BreadcrumbFilter());
                        options.Filters.Add(new OperationFilter());

                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(options => {
                        // we need this
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddApiVersioning(options => { options.Conventions.Add(new VersionByNamespaceConvention()); });

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddAntiforgery(options => { options.HeaderName = "xsrf-token"; });
        }

    }

}