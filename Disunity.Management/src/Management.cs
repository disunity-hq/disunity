using System;
using System.IO.Abstractions;
using System.Reflection;

using BindingAttributes;

using Disunity.Client.v1;
using Disunity.Management.Factories;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management {

    public class Management {


        protected Management() {
            
        }

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
            BindingAttribute.ConfigureBindings(services, new []{Assembly.GetExecutingAssembly()});
            FactoryAttribute.ConfigureFactories(services, new []{Assembly.GetExecutingAssembly()});
            services.ConfigureApiClient();
            services.AddSingleton(config);
            services.AddSingleton<Management>();
            services.AddSingleton<IFileSystem, FileSystem>();
        }

    }

}