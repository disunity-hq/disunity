using System.Collections.Generic;

using Disunity.Store.Startup.Binders;
using Disunity.Store.Startup.Services;

using EFCoreHooks.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Startup {

    public class Startup {

        private IConfiguration _config;
        private readonly ILogger<Startup> _logger;

        public Startup(ILoggerFactory logFactory, IConfiguration config, ILogger<Startup> logger) {
            _config = config;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.ConfigureAttributes();
            services.ConfigureAuthentication(_config);
            services.ConfigureAuthorization();
            services.ConfigureCookies();
            services.ConfigureDatabase(_config);
            services.ConfigureIdentity();
            services.ConfigureMvc();
            services.ConfigureSwagger();
            services.ConfigureRouting();
            services.ConfigureDbHooks();
            services.ConfigureAutoMapper();
            services.ConfigureStorageProvider(_config);
        }

        public void Configure(IApplicationBuilder app,
                              IEnumerable<IStartupService> startupServices) {
            foreach (var startupService in startupServices) {
                _logger.LogInformation($"Starting service: {startupService.GetType().Name}");
                startupService.Startup(app);
            }
        }

    }

}