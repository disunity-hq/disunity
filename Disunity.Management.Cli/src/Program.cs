using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CommandLine;

using Disunity.Core;
using Disunity.Management.Cli.Commands;
using Disunity.Management.Cli.Commands.Options;
using Disunity.Management.Cli.Services;


namespace Disunity.Management.Cli {

    public class Program {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        private static void Main(string[] args) {
            var program = new Program();
            program.ProcessArgs(args);
        }

        private Program() {
            _serviceProvider = SetupServiceProvider();
            _logger = _serviceProvider.GetRequiredService<ILogger>();
        }

        private void ProcessArgs(IEnumerable<string> args) {
            Parser.Default.ParseVerbs<TargetCommandOptions, ModCommandOptions, ProfileCommandOptions>(args)
                  .WithParsed(ExecuteCommand);
        }

        private IServiceProvider SetupServiceProvider() {
            var configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .Build();

            var services = new ServiceCollection();
            new Startup(configuration).ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            return provider;
        }

        private async void ExecuteCommand(object arg) {
            var commandOptions = (CommandOptionsBase) arg;
            var commandType = typeof(ICommandBase<>).MakeGenericType(commandOptions.GetType());

            object command;

            try {
                command = _serviceProvider.GetRequiredService(commandType);
            }
            catch (Exception ex) {
                _logger.LogError($"No command found to handle {commandType.Name}");
                _logger.LogError(ex.Message);
                return;
            }

            try {
                var method = commandType.GetMethod("Execute");
                var task = (Task) method.Invoke(command, new[] {commandOptions});
                await task;
            }
            catch (Exception ex) {
                _logger.LogError($"An error occured handling {command.GetType().Name}");
                _logger.LogError(ex.Message);
            }
        }

    }

}