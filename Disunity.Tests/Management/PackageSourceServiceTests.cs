using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Management.Models;
using Disunity.Management.Options;
using Disunity.Management.PackageSources;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Microsoft.Extensions.Options;

using Moq;

using Xunit;


namespace Disunity.Tests.Management {

    public class PackageSourceServiceTestsFixture {

        public List<string> PackageSources { get; }
        public PackageSourceService PackageSourceService { get; }
        public Mock<IPackageSourceFactory> MockSourceFactory { get; }
        public Mock<IPackageSource> MockSource { get; }

        public PackageSourceServiceTestsFixture() {
            PackageSources = new List<string> {
                "disunity://disunity.io/api/v1"
            };

            var options = new OptionsWrapper<ManagementOptions>(new ManagementOptions {
                PackageSources = PackageSources
            });

            MockSource = new Mock<IPackageSource>();
            var mockSource = MockSource.Object;

            MockSource.Setup(m => m.CanHandlePackage(It.IsAny<PackageIdentifier>())).Returns(Task.FromResult(true));

            MockSourceFactory = new Mock<IPackageSourceFactory>();
            var mockSourceFactory = MockSourceFactory.Object;

            MockSourceFactory
                .Setup(f => f.InstantiateSource(It.IsAny<string>()))
                .Returns((string uri) => new[] {mockSource});

            PackageSourceService = new PackageSourceService(options, mockSourceFactory);
        }

    }

    public class PackageSourceServiceTests : IClassFixture<PackageSourceServiceTestsFixture> {

        private readonly PackageSourceServiceTestsFixture _fixture;

        public PackageSourceServiceTests(PackageSourceServiceTestsFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanInitialize() {
            _fixture.MockSourceFactory.Verify(
                m => m.InstantiateSource(It.IsIn(_fixture.PackageSources.ToArray())),
                Times.Exactly(_fixture.PackageSources.Count));
        }

        [Fact]
        public async void CanGetImportStream() {

            var packageId = new DisunityDistroIdentifier() {Id = "disunity_1.0.0"};

            var actual = await _fixture.PackageSourceService.GetPackageImportStream(packageId);

            _fixture.MockSource.Verify(m => m.CanHandlePackage(packageId), Times.Once);
            _fixture.MockSource.Verify(m => m.GetPackageImportStream(packageId), Times.Once);
        }

    }

}