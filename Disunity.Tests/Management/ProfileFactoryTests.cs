using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Management;
using Disunity.Management.Factories;
using Disunity.Management.Models;

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

            var metaPath = Util.GetAbsolutePath("disunity", "managed", "risk-of-rain-2", "profiles", "default", "meta.json");

            FileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [metaPath] = new MockFileData(defaultProfileMeta)
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
        public async void CanLoadProfileFromFile() {
            var expected = new Profile {
                Path = Util.GetAbsolutePath("disunity", "managed", "risk-of-rain-2", "profiles", "default"),
                DisplayName = "Default"
            };

            var actual = await _fixture.ProfileFactory.Load(expected.Path);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CanCreateProfileDirectory_GeneratedDirectoryName() {
            const string displayName = "Test Profile w\\ith $trange n@ame";
            var path = Util.GetAbsolutePath("disunity", "managed", "risk-of-rain-2", "profiles");
            var created = await _fixture.ProfileFactory.Create(path, displayName);

            await AssertProfileCreated(created, displayName);
        }

        [Fact]
        public async void CanCreateProfileDirectory_SpecificDirectoryName() {
            const string displayName = "Test Profile w\\ith $trange n@ame";
            var path = Util.GetAbsolutePath("disunity", "managed", "risk-of-rain-2", "profiles", "strange");
            var created = await _fixture.ProfileFactory.CreateExactPath(path, displayName);

            await AssertProfileCreated(created, displayName);
        }

        private async Task AssertProfileCreated(Profile created, string displayName) {
            Assert.NotNull(created);
            Assert.Equal(displayName, created.DisplayName);
            Assert.False(string.IsNullOrEmpty(created.Path), "Expected path to have a value");

            var metaFilePath = _fixture.FileSystem.Path.Combine(created.Path, "meta.json");
            Util.AssertFileExists(_fixture.FileSystem, metaFilePath);

            var profilePath = _fixture.FileSystem.Path.GetDirectoryName(metaFilePath);
            var loadedProfile = await _fixture.ProfileFactory.Load(profilePath);

            Assert.Equal(created, loadedProfile);
        }

    }

}