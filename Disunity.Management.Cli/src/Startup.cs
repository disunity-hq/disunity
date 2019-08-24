using Disunity.Core;
using Disunity.Management.Cli.Commands;
using Disunity.Management.Cli.Commands.Options;
using Disunity.Management.Cli.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management.Cli {

    public class Startup {

        private readonly IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration) {
            _configuration = configuration;

        }

        public void ConfigureServices(ServiceCollection services) {
            services.AddSingleton<ILogger, ConsoleLogger>();
        }

    }

}
