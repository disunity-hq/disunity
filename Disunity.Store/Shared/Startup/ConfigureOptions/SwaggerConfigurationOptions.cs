using BindingAttributes;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Disunity.Store.Startup.ConfigureOptions {

    [AsTransient(typeof(IConfigureOptions<SwaggerGenOptions>))]
    public class SwaggerConfigurationOptions : IConfigureOptions<SwaggerGenOptions> {

        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerConfigurationOptions(IApiVersionDescriptionProvider provider) {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options) {
            foreach (var description in _provider.ApiVersionDescriptions) {
                options.SwaggerDoc(description.GroupName, new Info() {
                    Title = $"Disunity API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }
        }

    }

}