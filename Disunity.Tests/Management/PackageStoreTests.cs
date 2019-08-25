using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Client.v1.Models;
using Disunity.Management.PackageStores;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;


namespace Disunity.Tests.Management {

    public class PackageStoreFixture {

        // Totally not the real url but it doesn't matter
        public const string DisunityDistroDownloadBase = "https://disunity.io/distro/download";

        public MockBasePackageStore BasePackageStore { get; }
        public DisunityDistroStore DisunityDistroStore { get; }
        public ModPackageStore ModPackageStore { get; }

        public PackageStoreFixture() {
            var rootPath = Util.GetAbsolutePath();
            BaseStorePath = Path.Combine(rootPath, "disunity");

            var mockDisunityClient = Mock.Of<IDisunityClient>();
            var mockModListClient = Mock.Of<IModListClient>();
            var mockSymbolicLink = Mock.Of<ISymbolicLink>();
            var mockZipUtil = Mock.Of<IZipUtil>();
            MockPackageStore = new Mock<IPackageStore>();

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [Path.Combine(BaseStorePath, "test_package")] = new MockDirectoryData(),
                [Path.Combine(BaseStorePath, "disunity_1.0.0")] = new MockDirectoryData()
            });

            var services = new ServiceCollection();
            services.Configure<PackageStoreOptions>(options => { options.Path = BaseStorePath; });

            services.AddSingleton(typeof(IFileSystem), MockFileSystem)
                    .AddSingleton(typeof(ISymbolicLink), mockSymbolicLink)
                    .AddSingleton(typeof(IZipUtil), mockZipUtil)
                    .AddSingleton(typeof(IDisunityClient), mockDisunityClient)
                    .AddSingleton(MockPackageStore);

            services.AddSingleton<MockBasePackageStore>()
                    .AddSingleton<DisunityDistroStore>()
                    .AddSingleton<ModPackageStore>();

            var serviceProvider = services.BuildServiceProvider();

            BasePackageStore = serviceProvider.GetRequiredService<MockBasePackageStore>();
            DisunityDistroStore = serviceProvider.GetRequiredService<DisunityDistroStore>();
            ModPackageStore = serviceProvider.GetRequiredService<ModPackageStore>();

            MockDisunityClient = Mock.Get(mockDisunityClient);
            MockModListClient = Mock.Get(mockModListClient);
            MockSymbolicLink = Mock.Get(mockSymbolicLink);
            MockZipUtil = Mock.Get(mockZipUtil);

            MockDisunityClient.Setup(m => m.GetDisunityVersionsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(new List<DisunityVersionDto> {
                new DisunityVersionDto {
                    Url = DisunityDistroDownloadBase + "/1.0.0",
                    VersionNumber = "1.0.0"
                },
                new DisunityVersionDto {
                    Url = DisunityDistroDownloadBase + "/2.0.0",
                    VersionNumber = "2.0.0"
                }
            }));

            MockZipUtil.Setup(m => m.ExtractOnlineZip(It.IsAny<string>(), It.IsAny<string>()))
                       .Returns((string url, string path) => Task.FromResult(path));

            MockPackageStore.Setup(m => m.GetDownloadUrl(It.IsIn("disunity_1.0.0", "disunity_2.0.0"), It.IsAny<CancellationToken>()))
                            .Returns(((string package, CancellationToken cancellationToken) => Task.FromResult($"{DisunityDistroDownloadBase}/{package.Substring("disunity_".Length)}")));
        }

        public Mock<IModListClient> MockModListClient { get; }

        public Mock<ISymbolicLink> MockSymbolicLink { get; }

        public Mock<IDisunityClient> MockDisunityClient { get; }

        public Mock<IPackageStore> MockPackageStore { get; set; }

        public Mock<IZipUtil> MockZipUtil { get; set; }

        public string BaseStorePath { get; }

        public MockFileSystem MockFileSystem { get; }

        public void ResetMocks() {
            MockDisunityClient.Invocations.Clear();
            MockPackageStore.Invocations.Clear();
            MockSymbolicLink.Invocations.Clear();
            MockZipUtil.Invocations.Clear();
            MockModListClient.Invocations.Clear();
        }

    }

    public class PackageStoreTests : IClassFixture<PackageStoreFixture> {

        private readonly ITestOutputHelper _log;
        private readonly PackageStoreFixture _fixture;

        public PackageStoreTests(ITestOutputHelper log, PackageStoreFixture fixture) {
            _log = log;
            _fixture = fixture;

            _fixture.ResetMocks();
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

        [Theory]
        [InlineData("1.0.0", false)]
        [InlineData("2.0.0", true)]
        [InlineData("0.0.0", false, false)]
        public async Task CanDownloadPackage(string disunityVersion, bool shouldDownload, bool hasPath = true) {
            var packageName = $"disunity_{disunityVersion}";

            var expected = hasPath ? Path.Combine(_fixture.BaseStorePath, packageName) : null;
            var actual = await _fixture.BasePackageStore.DownloadPackage(packageName);

            _fixture.MockPackageStore.Verify(m => m.GetDownloadUrl(packageName, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(expected, actual);

            if (shouldDownload) {
                var expectedUrl = $"{PackageStoreFixture.DisunityDistroDownloadBase}/{disunityVersion}";
                var expectedPath = Path.Combine(_fixture.BaseStorePath, $"disunity_{disunityVersion}");
                _fixture.MockZipUtil.Verify(m => m.ExtractOnlineZip(expectedUrl, expectedPath), Times.Once);
            }
        }

        [Theory]
        [InlineData("2.0.0", PackageStoreFixture.DisunityDistroDownloadBase + "/2.0.0")]
        [InlineData("1.0.0", PackageStoreFixture.DisunityDistroDownloadBase + "/1.0.0")]
        [InlineData("0.0.0", null)]
        [InlineData("ahhhhhh", null)]
        public async void CanGetDisunityDownloadUrl(string disunityVersion, string expected) {

            var actual = await _fixture.DisunityDistroStore.GetDownloadUrl($"disunity_{disunityVersion}");

            Assert.Equal(expected, actual);
        }

    }

    public class MockBasePackageStore : BasePackageStore {

        private readonly Mock<IPackageStore> _mock;

        public override Task<string> GetDownloadUrl(string fullPackageName, CancellationToken cancellationToken = default) {
            return _mock.Object.GetDownloadUrl(fullPackageName, cancellationToken);
        }

        public MockBasePackageStore(IOptionsMonitor<PackageStoreOptions> optionsAccessor, IFileSystem fileSystem, ISymbolicLink symbolicLink, IZipUtil zipUtil, Mock<IPackageStore> mock) : base(optionsAccessor, fileSystem, symbolicLink, zipUtil) {
            _mock = mock;
        }

    }

}