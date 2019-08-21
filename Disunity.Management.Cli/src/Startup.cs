using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management.Cli {

    public class Startup {

        private readonly IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration) {
            _configuration = configuration;

        }

        public void ConfigureServices(ServiceCollection services) {
            
        }

    }

}