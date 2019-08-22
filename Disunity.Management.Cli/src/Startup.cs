using Disunity.Core;
using Disunity.Management.Cli.Services;

using Microsoft.Extensions.Configuration;

using Ninject.Modules;


namespace Disunity.Management.Cli {

    public class Startup : NinjectModule {

        private readonly IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration) {
            _configuration = configuration;

        }

        public override void Load() {
            Bind<ILogger>().To<ConsoleLogger>();
        }

    }

}