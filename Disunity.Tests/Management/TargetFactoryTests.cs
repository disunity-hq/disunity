using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Management;
using Disunity.Management.Factories;
using Disunity.Management.Models;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;

using Moq;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests.Management {

    public class TargetFactoryFixture {

        public TargetFactoryFixture() {
            Targets = new[] {
                new Target(new TargetMeta {
                    DisplayName = "Risk of Rain 2",
                    ExecutablePath = Util.GetAbsolutePath("Program Files", "Risk of Rain 2","RiskOfRain.exe"),
                    ManagedPath = Util.GetAbsolutePath("test", "managed", "risk-of-rain-2"),
                    Slug = "risk-of-rain-2"
                }),
                new Target(new TargetMeta {
                    DisplayName = "Stationeers",
                    ExecutablePath = Util.GetAbsolutePath("Program Files", "Stationeers","rocketstation.exe"),
                    ManagedPath = Util.GetAbsolutePath("test", "managed", "stationeers"),
                    Slug = "stationeeers"
                }) 
            };

            MockFileSystem = CreateMockFileSystem(Targets);

            MockProfileFactory = Mock.Of<IProfileFactory>();

            var mockSymbolicLink = Mock.Of<ISymbolicLink>();

            Mock.Get(MockProfileFactory).Setup(f => f.CreateExactPath(It.IsAny<string>(), "Default"))
                .Returns((string a, string b) => Task.FromResult(new Profile {Path = a, DisplayName = b}));

            var config = new ConfigurationBuilder()
                         .AddInMemoryCollection(new[] {new KeyValuePair<string, string>("ManagedRoot", "")})
                         .Build();

            TargetFactory = new TargetFactory(config, MockFileSystem, MockProfileFactory, mockSymbolicLink, new Crypto(null));
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
            var files = targets.ToDictionary(target => GetTargetInfoPath(target.TargetMeta.ManagedPath), MockFileDataFromSerializable);

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
                actual[i].TargetMeta.ManagedPath = expectedTargets[i].TargetMeta.ManagedPath;
            }

            actual.AssertItemsEqual(expectedTargets);
        }

//        [Fact]
//        public async void CanLoadFromFile() {
//            var expectedTarget = _fixture.Targets[0];
//
//            var actual = await _fixture.TargetFactory.FromFile(expectedTarget.MetaFilePath);
//            _log.WriteLine(expectedTarget.MetaFilePath);
//            Assert.NotNull(actual);
//
//            actual.TargetMeta.ManagedPath = expectedTarget.TargetMeta.ManagedPath;
//            Assert.Equal(expectedTarget, actual);
//        }

        [Fact]
        public async void CanStartTrackingTarget() {

            var expectedTarget = new Target(new TargetMeta() {
                DisplayName = "Cuphead",
                ExecutablePath = Util.GetAbsolutePath("Program Files", "Cuphead","Cuphead.exe"),
                ManagedPath = Util.GetAbsolutePath("test", "managed", "cuphead"),
                Slug = "cuphead"
            });

            var actual = await _fixture.TargetFactory.CreateManagedTarget(expectedTarget.TargetMeta.ExecutablePath, expectedTarget.TargetMeta.DisplayName, expectedTarget.TargetMeta.Slug, 0);

            Assert.Equal(expectedTarget, actual);

            var defaultProfilePath = _fixture.MockFileSystem.Path.Combine(expectedTarget.TargetMeta.ManagedPath, "profiles", "default");

            Mock.Get(_fixture.MockProfileFactory).Verify(f => f.CreateExactPath(defaultProfilePath, "Default"), Times.Once());

        }

        [Fact]
        public async void FromFileReturnsNullWhenFileIsNonExistent() {
            var actual = await _fixture.TargetFactory.FromFile(Util.GetAbsolutePath("does", "not", "exist"));
            Assert.Null(actual);
        }

    }

}