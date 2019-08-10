using System;

using BindingAttributes;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Startup.Services {

    [AsSingleton(typeof(IStartupService))]
    public class DatabaseService : IStartupService {

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration config,
                               ILogger<DatabaseService> logger,
                               IHostingEnvironment env) {

            _config = config;
            _logger = logger;
            _env = env;

        }

        public void Startup(IApplicationBuilder appBuilder) {
            try {
                using (var scope = appBuilder.ApplicationServices.CreateScope()) {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (_config.GetValue("Database.MigrateOnStartup", _env.IsDevelopment())) {
                        context.Database.Migrate();
                    }

                    if (_config.GetValue("Database.SeedOnStartup", _env.IsDevelopment())) {
                        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
                        seeder.Seed().Wait();
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

    }

}