using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using Disunity.Management;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class ProfileFixture {

        public Profile DefaultProfile { get; }

        public Target Target { get; }

        public IFileSystem MockFileSystem { get; }

        public ProfileFixture() {
            Target = new Target {
                Slug = "risk-of-rain-2",
                DisplayName = "Risk of Rain 2",
                ExecutableName = "Risk of Rain.exe",
                ManagedPath = @"C:\test\managed\risk-of-rain-2_1492FF6C8FD37B8D9BC9120CEF7A8409",
                TargetPath = @"C:\Program Files\Risk of Rain 2"
            };

            var defaultProfilePath = new MockFileSystem().Path.Combine(Target.ManagedPath, "profiles", "default");
            var defaultProfileMetaPath = new MockFileSystem().Path.Combine(defaultProfilePath, "meta.json");

            DefaultProfile = new Profile {
                Path = defaultProfilePath,
                DisplayName = "Default"
            };

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [defaultProfileMetaPath] = new MockFileData(JsonConvert.SerializeObject(DefaultProfile, Formatting.Indented))
            });

        }

    }

    public class ProfileTests : IClassFixture<ProfileFixture> {

        private readonly ProfileFixture _fixture;

        public ProfileTests(ProfileFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanDeleteProfile() {
            Util.AssertDirectoryExists(_fixture.MockFileSystem, _fixture.DefaultProfile.Path);
            
            _fixture.DefaultProfile.Delete(_fixture.MockFileSystem);
            
            Util.AssertDirectoryNotExists(_fixture.MockFileSystem, _fixture.DefaultProfile.Path);
        }

    }

}