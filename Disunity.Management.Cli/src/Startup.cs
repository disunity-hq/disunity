using System.IO.Abstractions;

using Disunity.Client;
using Disunity.Client.v1;
using Disunity.Core;
using Disunity.Management.Cli.Commands;
using Disunity.Management.Cli.Commands.Options;
using Disunity.Management.Cli.Commands.Target;
using Disunity.Management.Cli.Services;
using Disunity.Management.Factories;
using Disunity.Management.Models;
using Disunity.Management.PackageStores;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Disunity.Management.Cli {

    public class Startup {

        private readonly IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration) {
            _configuration = configuration;

        }

        public void ConfigureServices(ServiceCollection services) {
            services.AddSingleton(_configuration as IConfiguration);
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ICommandBase<TargetInitOptions>, InitCommand>();
            services.AddSingleton<IPackageStore, DisunityDistroStore>();
            services.AddSingleton<ISymbolicLink, SymbolicLink>();
            services.AddSingleton<IZipUtil, ZipUtil>();
            services.AddSingleton<ITargetFactory, TargetFactory>();
            services.AddSingleton<IProfileFactory, ProfileFactory>();
            services.ConfigureApiClient();
        }

    }

}
