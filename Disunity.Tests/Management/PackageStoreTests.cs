using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Management.PackageStores;
using Disunity.Management.Util;

using Moq;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;


namespace Disunity.Tests.Management {

    public class PackageStoreFixture {

        public MockBasePackageStore BasePackageStore { get; }
        public DisunityDistroStore DisunityDistroStore { get; }
        public ModPackageStore ModPackageStore { get; }

        public PackageStoreFixture() {
            var rootPath = Util.GetAbsolutePath();
            BaseStorePath = Path.Combine(rootPath, "mock");
            DisunityStorePath = Path.Combine(rootPath, "disunity");
            ModStorePath = Path.Combine(rootPath, "mods");

            var mockDisunityClient = Mock.Of<IDisunityClient>();
            var mockModListClient = Mock.Of<IModListClient>();
            var mockSymbolicLink = Mock.Of<ISymbolicLink>();

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [Path.Combine(BaseStorePath, "test_package")] = new MockDirectoryData(),
                [Path.Combine(DisunityStorePath, "disunity_1.0.0")] = new MockDirectoryData()
            });

            BasePackageStore = new MockBasePackageStore(BaseStorePath, MockFileSystem, mockSymbolicLink);
            DisunityDistroStore = new DisunityDistroStore(DisunityStorePath, MockFileSystem, mockSymbolicLink, mockDisunityClient);
            ModPackageStore = new ModPackageStore(ModStorePath, MockFileSystem, mockSymbolicLink, mockModListClient);

            MockDisunityClient = Mock.Get(mockDisunityClient);
            MockModListClient = Mock.Get(mockModListClient);
            MockSymbolicLink = Mock.Get(mockSymbolicLink);
        }

        public Mock<IModListClient> MockModListClient { get; }

        public Mock<ISymbolicLink> MockSymbolicLink { get; }

        public Mock<IDisunityClient> MockDisunityClient { get; }

        public string BaseStorePath { get; }

        public string DisunityStorePath { get; }

        public string ModStorePath { get; }

        public MockFileSystem MockFileSystem { get; }

    }

    public class PackageStoreTests : IClassFixture<PackageStoreFixture> {

        private readonly ITestOutputHelper _log;
        private readonly PackageStoreFixture _fixture;

        public PackageStoreTests(ITestOutputHelper log, PackageStoreFixture fixture) {
            _log = log;
            _fixture = fixture;

            var foo = new IPackageStore[] {_fixture.BasePackageStore, _fixture.DisunityDistroStore, _fixture.ModPackageStore};
        }

        [Theory]
        [InlineData("test_package", true)]
        [InlineData("does_not_exist", false)]
        public void CanGetPathForPackage(string package, bool shouldExist) {
            var actual = _fixture.BasePackageStore.GetPackagePath(package);

            if (shouldExist) {
                Assert.Equal(actual, Path.Combine(_fixture.BaseStorePath, package));
            } else {
                Assert.Null(actual);
            }
        }

        public static IEnumerable<object[]> CanCreateSymbolicLink_Data => new List<object[]> {
            new object[] {"test_package", Util.GetAbsolutePath("some", "valid", "path")}
        };

        [Theory]
        [MemberData(nameof(CanCreateSymbolicLink_Data))]
        public async void CanCreateSymbolicLink(string package, string path) {
            var targetPath = _fixture.BasePackageStore.GetPackagePath(package);
            await _fixture.BasePackageStore.CreatePackageReference(package, path);
            _fixture.MockSymbolicLink.Verify(m => m.CreateDirectoryLink(path, targetPath), Times.Once());
        }

        [Fact]
        public async void CanWipeStore() {
            await _fixture.BasePackageStore.Clear();
            Util.AssertDirectoryNotExists(_fixture.MockFileSystem, _fixture.BaseStorePath);
        }

//        [Fact]
        public async void CanDownloadDisunityDistro() {
            const string disunityPackage = "disunity_2.0.0";

            var expected = Path.Combine(_fixture.DisunityStorePath, disunityPackage);
            var actual = await _fixture.DisunityDistroStore.DownloadPackage(disunityPackage);

            Assert.Equal(expected, actual);
        }

    }

    public class MockBasePackageStore : BasePackageStore {

        public MockBasePackageStore(string rootPath, IFileSystem fileSystem, ISymbolicLink symbolicLink) : base(rootPath, fileSystem, symbolicLink) { }

        public override Task<string> DownloadPackage(string fullPackageName, bool force = false) {
            throw new System.NotImplementedException();
        }

    }

}