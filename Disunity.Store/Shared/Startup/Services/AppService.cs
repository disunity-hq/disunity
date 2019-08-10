using System;

using BindingAttributes;

using Disunity.Store.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Syncfusion.Licensing;


namespace Disunity.Store.Startup.Services {

    [AsSingleton(typeof(IStartupService))]
    public class AppService : IStartupService {

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private readonly IServiceProvider _services;

        private IApplicationBuilder _app;

        public AppService(IConfiguration config,
                          IHostingEnvironment env,
                          IServiceProvider services) {
            _config = config;
            _env = env;
            _services = services;

        }

        public void Startup(IApplicationBuilder appBuilder) {
            _app = appBuilder;

            SyncfusionLicenseProvider.RegisterLicense(_config["Syncfusion:License"]);

            EnvironmentStartup();

            _app.UseStaticFiles();
            _app.UseCookiePolicy();
            _app.UseAuthentication();

            _app.UseMiddleware<DownloadRedirectMiddleware>();

            _app.UseMvc(routes => {
                routes.MapAreaRoute("api", "API",
                                    "api/v{version:apiVersion}/[controller]/[action=Index]");
            });

        }

        public void DevelopmentStartup() {
            _app.UseDeveloperExceptionPage();
            _app.UseDatabaseErrorPage();
        }

        public void ProductionStartup() {
            _app.UseHttpsRedirection();
            _app.UseExceptionHandler("/Error");
            _app.UseHsts(); // see https://aka.ms/aspnetcore-hsts.
        }

        public void EnvironmentStartup() {
            if (_env.IsDevelopment()) {
                DevelopmentStartup();
            } else {
                ProductionStartup();
            }
        }

    }

}