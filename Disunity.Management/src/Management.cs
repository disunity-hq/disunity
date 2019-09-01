using System;
using System.IO.Abstractions;
using System.Reflection;

using BindingAttributes;

using Disunity.Client.v1;
using Disunity.Management.Data;
using Disunity.Management.Managers;
using Disunity.Management.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management {

    /// <summary>
    /// Entry point into the Disunity.Management layer. 
    /// </summary>
    /// <remarks>
    /// Please use the static <see cref="Create"/> method to get instances of the Management class.
    /// </remarks>
    [AsSingleton]
    public class Management {

        public ITargetManager TargetManager { get; }

        public Management(ManagementDbContext context, ITargetManager targetManager) {
            TargetManager = targetManager;
            context.Database.Migrate();
        }

        /// <summary>
        /// Configures all internal services and creates an instances of <see cref="Management"/>
        /// </summary>
        /// <param name="config">The configuration to use for this instance</param>
        /// <returns></returns>
        public static Management Create(IConfiguration config) {
            var serviceProvider = BuildServiceProvider(config);

            return serviceProvider.GetRequiredService<Management>();
        }

        private static IServiceProvider BuildServiceProvider(IConfiguration config) {
            var services = new ServiceCollection();
            ConfigureServices(services, config);

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration config) {
            var assemblies = new[] {Assembly.GetExecutingAssembly()};
            BindingAttribute.ConfigureBindings(services, assemblies);
            OptionsAttribute.ConfigureOptions(services, config, assemblies);
            FactoryAttribute.ConfigureFactories(services, assemblies);
            services.ConfigureApiClient();
            services.AddSingleton(config);
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddDbContext<ManagementDbContext>(options => { options.UseSqlite("Data Source=disunity.db"); }, ServiceLifetime.Singleton);
        }

    }

}
