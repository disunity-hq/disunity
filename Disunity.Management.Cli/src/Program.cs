using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using CommandLine;

using Disunity.Core;
using Disunity.Management.Cli.Commands;

using Ninject;


namespace Disunity.Management.Cli {

    public class Program : IDisposable {

        private readonly ILogger _logger;
        private int _exitCode;
        private readonly IKernel _kernel;

        private static int Main(string[] args) {
            using (var program = new Program()) {
                program.ProcessArgs(args);

                return program._exitCode;
            }
        }

        private Program() {
            _kernel = CreateKernel();
            _logger = _kernel.Get<ILogger>();
            _exitCode = 0;
        }

        private void ProcessArgs(IEnumerable<string> args) {
            Parser.Default.ParseVerbs<LogCommand>(args)
                  .WithParsed(ExecuteCommand);
        }

        private IKernel CreateKernel() {
            var configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .Build();

            var kernel = new StandardKernel(new Startup(configuration));

            return kernel;
        }

        private async void ExecuteCommand(object arg) {
            var command = (CommandBase) arg;

            try {
                // Inject dependencies into command
                _kernel.Inject(command);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to resolve dependencies for {command.GetType().Name}");
                _logger.LogError(ex.Message);
                _exitCode = 1;
                return;
            }

            try {
                await command.Execute();
            }
            catch (Exception ex) {
                _logger.LogError($"An error occured handling {command.GetType().Name}");
                _logger.LogError(ex.Message);

                if (command.Verbose) {
                    _logger.LogError(ex.ToString());
                }
                _exitCode = 1;
            }
        }

        public void Dispose() {
            _kernel?.Dispose();
        }

    }

}