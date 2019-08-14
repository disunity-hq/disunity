using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using Disunity.Management;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class ProfileFactoryFixture {

        public ProfileFactory ProfileFactory { get; }

        public IFileSystem FileSystem { get; }

        public ProfileFactoryFixture() {

            var defaultProfileMeta = JsonConvert.SerializeObject(new Profile {
                DisplayName = "Default"
            });

            FileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [@"c:\disunity\managed\risk-of-rain-2\profiles\default\meta.json"] = new MockFileData(defaultProfileMeta)
            });

            ProfileFactory = new ProfileFactory(FileSystem);

        }

    }

    public class ProfileFactoryTests : IClassFixture<ProfileFactoryFixture> {

        private readonly ProfileFactoryFixture _fixture;

        public ProfileFactoryTests(ProfileFactoryFixture fixture) {
            _fixture = fixture;

        }

        [Fact]
        public void CanLoadProfileFromFile() {
            var expected = new Profile {
                Path = @"c:\disunity\managed\risk-of-rain-2\profiles\default",
                DisplayName = "Default"
            };

            var actual = _fixture.ProfileFactory.Load(@"c:\disunity\managed\risk-of-rain-2\profiles\default");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateProfileDirectory_GeneratedDirectoryName() {
            const string displayName = "Test Profile w\\ith $trange n@ame";
            var created = _fixture.ProfileFactory.Create(@"c:\disunity\managed\risk-of-rain-2\profiles", displayName);

            AssertProfileCreated(created, displayName);
        }

        [Fact]
        public void CanCreateProfileDirectory_SpecificDirectoryName() {
            const string displayName = "Test Profile w\\ith $trange n@ame";
            var created = _fixture.ProfileFactory.CreateExactPath(@"c:\disunity\managed\risk-of-rain-2\profiles\strange", displayName);

            AssertProfileCreated(created, displayName);
        }

        private void AssertProfileCreated(Profile created, string displayName) {
            Assert.NotNull(created);
            Assert.Equal(displayName, created.DisplayName);
            Assert.False(string.IsNullOrEmpty(created.Path), "Expected path to have a value");

            var metaFilePath = _fixture.FileSystem.Path.Combine(created.Path, "meta.json");
            Util.AssertFileExists(_fixture.FileSystem, metaFilePath);

            var profilePath = _fixture.FileSystem.Path.GetDirectoryName(metaFilePath);
            var loadedProfile = _fixture.ProfileFactory.Load(profilePath);

            Assert.Equal(created, loadedProfile);
        }

    }

}