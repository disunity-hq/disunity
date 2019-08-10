using System;
using System.IO;
using System.Reflection;

using Disunity.Store.Startup.Filters;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class SwaggerBinderExt {

        public static void ConfigureSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(swagger => {
                swagger.OperationFilter<SwaggerDefaultValues>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });
        }

    }

}