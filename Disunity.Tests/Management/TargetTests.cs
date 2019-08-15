using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using Disunity.Management;
using Disunity.Management.Util;

using Moq;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class TargetFixture {

        public Profile DefaultProfile { get; }

        public Target Target { get; }

        public IFileSystem MockFileSystem { get; }
        public ISymbolicLink MockSymbolicLink { get; }

        public TargetFixture() {
            Target = new Target {
                Slug = "risk-of-rain-2",
                DisplayName = "Risk of Rain 2",
                ExecutableName = "Risk of Rain.exe",
                ManagedPath = @"C:\test\managed\risk-of-rain-2_1492FF6C8FD37B8D9BC9120CEF7A8409",
                TargetPath = @"C:\Program Files\Risk of Rain 2"
            };

            var targetMetaPath = Path.Combine(Target.ManagedPath, "target-info.json");
            var defaultProfilePath = Path.Combine(Target.ManagedPath, "profiles", "default");

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [defaultProfilePath] = new MockDirectoryData(),
                [targetMetaPath] = new MockFileData(JsonConvert.SerializeObject(Target))
            });

            DefaultProfile = new Profile {
                Path = defaultProfilePath,
                DisplayName = "Default"
            };

            MockSymbolicLink = Mock.Of<ISymbolicLink>();
        }

    }

    public class TargetTests : IClassFixture<TargetFixture> {

        private readonly TargetFixture _fixture;

        public TargetTests(TargetFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanSetActiveProfile() {

            _fixture.Target.SetActiveProfile(_fixture.MockFileSystem, _fixture.MockSymbolicLink, _fixture.DefaultProfile);

            var activeProfilePath = _fixture.MockFileSystem.Path.Combine(_fixture.Target.ManagedPath, "profiles", "active");

            Mock.Get(_fixture.MockSymbolicLink).Verify(s => s.CreateDirectoryLink(activeProfilePath, _fixture.DefaultProfile.Path), Times.Once());

        }

        [Fact]
        public void RemovesOldLinkIfExists() {
            var activeProfilePath = _fixture.MockFileSystem.Path.Combine(_fixture.Target.ManagedPath, "profiles", "active");
            _fixture.MockFileSystem.File.Create(activeProfilePath).Close();
            
            _fixture.Target.SetActiveProfile(_fixture.MockFileSystem, _fixture.MockSymbolicLink, _fixture.DefaultProfile);
            
            Util.AssertFileNotExists(_fixture.MockFileSystem, activeProfilePath);
        }
        
        [Fact]
        public void CanDeleteTarget() {
            Util.AssertDirectoryExists(_fixture.MockFileSystem, _fixture.Target.ManagedPath);

            _fixture.Target.Delete(_fixture.MockFileSystem);
            
            Util.AssertDirectoryNotExists(_fixture.MockFileSystem, _fixture.Target.ManagedPath);
        }

    }

}