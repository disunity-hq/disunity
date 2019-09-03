using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Client.v1.Models;
using Disunity.Management.Models;
using Disunity.Management.PackageSources;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.Protected;

using Xunit;


namespace Disunity.Tests.Management {

    public class PackageSourceTestsFixture {

        public PackageSourceTestsFixture() { }

    }

    public class PackageSourceTests : IClassFixture<PackageSourceTestsFixture> {

        private const string _disunityDownloadUrl = "https://disunity.io/distro/1.0.0/download";
        private const string _modDownloadUrl = "https://disunity.io/u/disunity-team/example-mod/1.0.0/download";

        private readonly PackageSourceTestsFixture _fixture;
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IDisunityClient> _mockDisunityClient;
        private readonly Mock<IModListClient> _mockModListClient;
        private readonly Mock<HttpMessageHandler> _mockMessageHandler;

        public PackageSourceTests(PackageSourceTestsFixture fixture) {
            _fixture = fixture;

            _mockMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _mockMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                                                  ItExpr.IsAny<HttpRequestMessage>(),
                                                  ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK
                });

            var mockHttpClient = new HttpClient(_mockMessageHandler.Object);

            _mockDisunityClient = new Mock<IDisunityClient>();

            _mockDisunityClient.Setup(c => c.GetDisunityVersionsAsync()).Returns(Task.FromResult(new List<DisunityVersionDto> {
                new DisunityVersionDto {VersionNumber = "1.0.0", Url = _disunityDownloadUrl}
            }));

            _mockModListClient = new Mock<IModListClient>();

            _mockModListClient.Setup(c => c.GetModsAsync(It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.FromResult(new List<ModDto> {
                new ModDto {
                    Owner = new OrgDto {
                        Slug = "disunity-team"
                    },
                    Slug = "example-mod",
                    Versions = new List<ModVersionDto> {
                        new ModVersionDto {
                            VersionNumber = "1.0.0", FileUrl = _modDownloadUrl
                        }
                    }
                }
            }));

            _mockApiClient = new Mock<IApiClient>();
            _mockApiClient.Setup(c => c.DisunityClient).Returns(_mockDisunityClient.Object);
            _mockApiClient.Setup(c => c.ModListClient).Returns(_mockModListClient.Object);
            _mockApiClient.Setup(c => c.HttpClient).Returns(mockHttpClient);
        }

        [Theory]
        [MemberData(nameof(DisunityImportSourceData))]
        public async void DisunitySourceCanGetImportStream(PackageIdentifier packageId, string expectedDownloadUrl) {
            var disunitySource = new DisunityPackageSource(_mockApiClient.Object);
            var sourceStream = await disunitySource.GetPackageImportStream(packageId);

            if (expectedDownloadUrl == null) {
                Assert.Null(sourceStream);
            } else {
                Assert.NotNull(sourceStream);

                _mockMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                                                      req.Method == HttpMethod.Get &&
                                                      req.RequestUri == new Uri(expectedDownloadUrl)),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
        }

        public static IEnumerable<object[]> DisunityImportSourceData => new List<object[]> {
            new object[] {new DisunityDistroIdentifier("1.0.0"), _disunityDownloadUrl},
            new object[] {new DisunityDistroIdentifier("2.0.0"), null},
            new object[] {new ModIdentifier("disunity-team", "example-mod", "1.0.0"), _modDownloadUrl},
            new object[] {new ModIdentifier("disunity-team", "example-mod", "1.1.0"), null},
            new object[] {new ModIdentifier("disunity-team", "example-mode", "1.0.0"), null},
        };

    }

}