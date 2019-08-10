using BindingAttributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Startup.Services {

    [AsSingleton(typeof(IStartupService))]
    public class SwaggerService : IStartupService {

        private readonly IApiVersionDescriptionProvider _descriptionProvider;
        private readonly ILogger<SwaggerService> _logger;

        public SwaggerService(IApiVersionDescriptionProvider descriptionProvider, ILogger<SwaggerService> logger) {
            _descriptionProvider = descriptionProvider;
            _logger = logger;
        }

        public void Startup(IApplicationBuilder app) {
            app.UseSwagger(c => { c.RouteTemplate = "/api/docs/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c => {
                c.RoutePrefix = "api/docs";

                foreach (var description in _descriptionProvider.ApiVersionDescriptions) {
                    _logger.LogInformation($"Registering API docs {description.GroupName}");

                    c.SwaggerEndpoint($"/api/docs/{description.GroupName}/swagger.json",
                                      description.GroupName.ToUpperInvariant());
                }
            });
        }

    }

}