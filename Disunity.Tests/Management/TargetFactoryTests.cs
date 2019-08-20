using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Management;
using Disunity.Management.Factories;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Moq;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests.Management {

    public class TargetFactoryFixture {

        public TargetFactoryFixture() {
            Targets = new[] {
                new Target {
                    DisplayName = "Risk of Rain 2",
                    ExecutableName = "RiskOfRain.exe",
                    TargetPath = Util.GetAbsolutePath("Program Files", "Risk of Rain 2"),
                    ManagedPath = Util.GetAbsolutePath("test", "managed", "risk-of-rain-2"),
                    Slug = "risk-of-rain-2"
                },
                new Target {
                    DisplayName = "Stationeers",
                    ExecutableName = "rocketstation.exe",
                    TargetPath = Util.GetAbsolutePath("Program Files", "Stationeers"),
                    ManagedPath = Util.GetAbsolutePath("test", "managed", "stationeers"),
                    Slug = "stationeeers"
                }
            };

            MockFileSystem = CreateMockFileSystem(Targets);

            MockProfileFactory = Mock.Of<IProfileFactory>();

            var mockSymbolicLink = Mock.Of<ISymbolicLink>();

            Mock.Get(MockProfileFactory).Setup(f => f.CreateExactPath(It.IsAny<string>(), "Default"))
                .Returns((string a, string b) => Task.FromResult(new Profile {Path = a, DisplayName = b}));

            TargetFactory = new TargetFactory(MockFileSystem, MockProfileFactory, mockSymbolicLink) {
                ManagedRoot = Util.GetAbsolutePath("test", "managed")
            };
        }

        public TargetFactory TargetFactory { get; }

        public IList<Target> Targets { get; }

        public IFileSystem MockFileSystem { get; }

        public IProfileFactory MockProfileFactory { get; }

        private static string GetTargetInfoPath(string directory) {
            return Path.Combine(directory, "target-info.json");
        }

        private static MockFileData MockFileDataFromSerializable(object obj) {
            return new MockFileData(JsonConvert.SerializeObject(obj));
        }

        private static IFileSystem CreateMockFileSystem(IEnumerable<Target> targets) {
            var files = targets.ToDictionary(target => GetTargetInfoPath(target.ManagedPath), MockFileDataFromSerializable);

            return new MockFileSystem(files);
        }

    }

    public class TargetFactoryTests : IClassFixture<TargetFactoryFixture> {

        public TargetFactoryTests(TargetFactoryFixture fixture, ITestOutputHelper log) {
            _fixture = fixture;
            _log = log;
        }

        private readonly TargetFactoryFixture _fixture;
        private readonly ITestOutputHelper _log;

        [Fact]
        public async void CanLoadAllFromDirectory() {
            var expectedTargets = new List<Target> {_fixture.Targets[0], _fixture.Targets[1]};

            var actual = await _fixture.TargetFactory.LoadAllFromPath(Util.GetAbsolutePath("test", "managed"));

            for (var i = 0; i < expectedTargets.Count; i++) {
                actual[i].ManagedPath = expectedTargets[i].ManagedPath;
            }

            actual.AssertItemsEqual(expectedTargets);
        }

        [Fact]
        public async void CanLoadFromFile() {
            var expectedTarget = _fixture.Targets[0];

            var actual = await _fixture.TargetFactory.FromFile(expectedTarget.MetaFilePath);
            _log.WriteLine(expectedTarget.MetaFilePath);
            Assert.NotNull(actual);
            
            actual.ManagedPath = expectedTarget.ManagedPath;
            Assert.Equal(expectedTarget, actual);
        }

        [Fact]
        public async void CanStartTrackingTarget() {
            var expectedTarget = new Target {
                DisplayName = "Cuphead",
                ExecutableName = "Cuphead.exe",
                TargetPath = Util.GetAbsolutePath("Program Files", "Cuphead"),
                ManagedPath = Util.GetAbsolutePath("test", "managed", "cuphead"),
                Slug = "cuphead"
            };

            var actual = await _fixture.TargetFactory.CreateManagedTarget(expectedTarget.ExecutablePath, expectedTarget.DisplayName, expectedTarget.Slug, 0);

            Assert.Equal(expectedTarget, actual);

            var defaultProfilePath = _fixture.MockFileSystem.Path.Combine(expectedTarget.ManagedPath, "profiles", "default");

            Mock.Get(_fixture.MockProfileFactory).Verify(f => f.CreateExactPath(defaultProfilePath, "Default"), Times.Once());

        }

        [Fact]
        public async void FromFileReturnsNullWhenFileIsNonExistent() {
            var actual = await _fixture.TargetFactory.FromFile(Util.GetAbsolutePath("does", "not", "exist"));
            Assert.Null(actual);
        }

    }

}