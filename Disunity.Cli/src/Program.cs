using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;

namespace Disunity.Cli
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static void ParseArgs(IEnumerable<string> args) {
            
        }

        private static IServiceProvider SetupServiceProvider() {
            var configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .Build();

            var services = new ServiceCollection();
            new Startup(configuration).ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            return provider;
        }
        
        
    }
}
