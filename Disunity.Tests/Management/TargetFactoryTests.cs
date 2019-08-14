using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using Disunity.Management;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class TargetFactoryFixture {

        public TargetFactoryFixture() {
            RoR2Target = new Target {
                DisplayName = "Risk of Rain 2",
                ExecutableName = "RiskOfRain.exe",
                TargetPath = @"C:\Program Files\Risk of Rain 2"
            };

            StationeersTarget = new Target {
                DisplayName = "Stationeers",
                ExecutableName = "rocketstation.exe",
                TargetPath = @"C:\Program Files\Stationeers"
            };

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [@"C:\test\managed\risk-of-rain-2\target-info.json"] = new MockFileData(JsonConvert.SerializeObject(RoR2Target)),
                [@"C:\test\managed\stationeers\target-info.json"] = new MockFileData(JsonConvert.SerializeObject(StationeersTarget)),
            });

            TargetFactory = new TargetFactory(fileSystem);
        }

        public TargetFactory TargetFactory { get; }

        public Target RoR2Target { get; }
        public Target StationeersTarget { get; }

    }

    public class TargetFactoryTests : IClassFixture<TargetFactoryFixture> {

        private readonly TargetFactoryFixture _fixture;

        public TargetFactoryTests(TargetFactoryFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanParseJson() {
            var expectedTarget = _fixture.RoR2Target;

            var json = JsonConvert.SerializeObject(expectedTarget);

            var actual = _fixture.TargetFactory.FromJson(json);

            Assert.Equal(expectedTarget, actual);
        }

        [Fact]
        public void CanLoadFromFile() {
            var expectedTarget = _fixture.RoR2Target;

            var actual = _fixture.TargetFactory.FromFile(@"C:\test\managed\risk-of-rain-2\target-info.json");

            Assert.Equal(expectedTarget, actual);
        }

        [Fact]
        public void CanLoadAllFromDirectory() {
            var expectedTargets = new List<Target> {_fixture.RoR2Target, _fixture.StationeersTarget};

            var actual = _fixture.TargetFactory.LoadAllFromPath(@"C:\test\managed");
            
            actual.AssertItemsEqual(expectedTargets);
        }

    }

}