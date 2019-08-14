using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using Disunity.Management;

using Moq;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class TargetFactoryFixture {

        public TargetFactory TargetFactory { get; }

        public IList<Target> Targets { get; }

        public IFileSystem MockFileSystem { get; }
        
        public IProfileFactory MockProfileFactory { get; }

        public TargetFactoryFixture() {
            Targets = new[] {
                new Target {
                    DisplayName = "Risk of Rain 2",
                    ExecutableName = "RiskOfRain.exe",
                    TargetPath = @"C:\Program Files\Risk of Rain 2",
                    ManagedPath = @"C:\test\managed\risk-of-rain-2",
                    Slug = "risk-of-rain-2"
                },
                new Target {
                    DisplayName = "Stationeers",
                    ExecutableName = "rocketstation.exe",
                    TargetPath = @"C:\Program Files\Stationeers",
                    ManagedPath = @"C:\test\managed\stationeers",
                    Slug = "stationeeers"
                }
            };

            MockFileSystem = CreateMockFileSystem(Targets);

            MockProfileFactory = Mock.Of<IProfileFactory>();

            TargetFactory = new TargetFactory(MockFileSystem, MockProfileFactory) {
                ManagedRoot = @"C:\test\managed"
            };
        }

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

        private readonly TargetFactoryFixture _fixture;

        public TargetFactoryTests(TargetFactoryFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanLoadFromFile() {
            var expectedTarget = _fixture.Targets[0];

            var actual = _fixture.TargetFactory.FromFile(@"C:\test\managed\risk-of-rain-2\target-info.json");
            actual.ManagedPath = expectedTarget.ManagedPath;

            Assert.Equal(expectedTarget, actual);
        }

        [Fact]
        public void FromFileReturnsNullWhenFileIsNonExistent() {
            var actual = _fixture.TargetFactory.FromFile(@"C:\does\not\exist");
            Assert.Null(actual);
        }

        [Fact]
        public void CanLoadAllFromDirectory() {
            var expectedTargets = new List<Target> {_fixture.Targets[0], _fixture.Targets[1]};

            var actual = _fixture.TargetFactory.LoadAllFromPath(@"C:\test\managed");

            for (var i = 0; i < expectedTargets.Count; i++) {
                actual[i].ManagedPath = expectedTargets[i].ManagedPath;
            }

            actual.AssertItemsEqual(expectedTargets);
        }
        
        [Fact]
        public void CanStartTrackingTarget() {
            var expectedTarget = new Target {
                DisplayName = "Cuphead",
                ExecutableName = "Cuphead.exe",
                TargetPath = @"C:\Program Files\Cuphead",
                ManagedPath = @"C:\test\managed\cuphead_C1B753477FAF53448FE078E5830439C4",
                Slug = "cuphead"
            };

            var actual = _fixture.TargetFactory.CreateManagedTarget(expectedTarget.ExecutablePath, expectedTarget.DisplayName, expectedTarget.Slug);

            Assert.Equal(expectedTarget, actual);

            var defaultProfilePath = _fixture.MockFileSystem.Path.Combine(expectedTarget.ManagedPath, "profiles", "default");
            
            Mock.Get(_fixture.MockProfileFactory).Verify(f => f.CreateExactPath(defaultProfilePath, "Default"), Times.Once());
            

//            var activeProfilePath = _fixture.FileSystem.Path.Combine(expectedTarget.ManagedPath, "profiles", "active");
//
//            var expectedFiles = new[] {
//                activeProfilePath,
//                @"C:\test\managed\Cuphead\profiles\default\meta.json",
//            };
//
//            foreach (var file in expectedFiles) {
//                Util.AssertFileExists(_fixture.FileSystem, file);
//            }
//
//
//            Assert.True((_fixture.FileSystem.File.GetAttributes(expectedFiles[0]) & FileAttributes.ReparsePoint) != 0,
//                        $"Expected {expectedFiles[0]} to be a symlink to {expectedFiles[1]}");
        }

    }

}
