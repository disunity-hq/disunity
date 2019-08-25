using System.IO.Abstractions.TestingHelpers;

using Disunity.Client.v1;
using Disunity.Core;
using Disunity.Management.Cli.Commands.Options;
using Disunity.Management.Cli.Commands.Target;
using Disunity.Management.Factories;
using Disunity.Management.PackageStores;

using Moq;

using Xunit;


namespace Disunity.Tests.Management.Cli {

    public class TargetInitFixutre {

        public InitCommand InitCommand { get; }

        public TargetInitFixutre() {
            var logger = Mock.Of<ILogger>();

            InitCommand = new InitCommand(logger, new MockFileSystem(),
                                          Mock.Of<ITargetClient>(), Mock.Of<IPackageStore>(), Mock.Of<ITargetFactory>(), Mock.Of<IDisunityClient>());
        }

    }

    public class TargetInitTests : IClassFixture<TargetInitFixutre> {

        private readonly TargetInitFixutre _fixutre;

        public TargetInitTests(TargetInitFixutre fixutre) {
            _fixutre = fixutre;
        }

        public async void CanInitTarget() {
            var options = new TargetInitOptions { };
            await _fixutre.InitCommand.Execute(options);
        }

    }

}