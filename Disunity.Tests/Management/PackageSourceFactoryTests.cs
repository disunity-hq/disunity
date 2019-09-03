using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Disunity.Management.Extensions;
using Disunity.Management.Models;
using Disunity.Management.Options;
using Disunity.Management.PackageSources;
using Disunity.Management.Util;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;


namespace Disunity.Tests.Management {

    public class PackageSourceFactoryTestsFixture {

        public PackageSourceFactory PackageSourceFactory { get; }

        public PackageSourceFactoryTestsFixture() {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();
            PackageSourceFactory = new PackageSourceFactory(serviceProvider, Assembly.GetExecutingAssembly());
        }

    }

    public class PackageSourceFactoryTests : IClassFixture<PackageSourceFactoryTestsFixture> {

        private readonly PackageSourceFactoryTestsFixture _fixture;

        public PackageSourceFactoryTests(PackageSourceFactoryTestsFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void CanLoadSourceTypes() {
            var expected = new Dictionary<string, List<Type>> {
                ["test://"] = new List<Type> {typeof(TestPackageSource)},
                ["disunity://"] = new List<Type> {typeof(TestDisunityPackageSource), typeof(TestDisunity2PackageSource)}
            };

            var actual = PackageSourceAttribute.GetAvailableSourceTypes(Assembly.GetExecutingAssembly());

            Assert.Equal(expected.Count, actual.Count);

            Assert.All(actual, pair => {
                var (key, value) = pair;
                Assert.True(expected.ContainsKey(key));
                Assert.Equal(expected[key], value);
            });
        }

        [Theory]
        [InlineData("test://", new[] {typeof(TestPackageSource)})]
        [InlineData("test://some/url/with/segments", new[] {typeof(TestPackageSource)})]
        [InlineData("disunity://disunity.io/api/v1", new[] {typeof(TestDisunityPackageSource), typeof(TestDisunity2PackageSource)})]
        public void CanFindMatchingSourceTypes(string uri, Type[] expectedMatches) {
            var actual = _fixture.PackageSourceFactory.FindMatchingSourceTypes(uri);

            Assert.Equal(expectedMatches, actual);
        }

        [Fact]
        public void CanInstantiateSources() {
            var expectedSourceMap = new Dictionary<string, Type[]> {
                ["test://foo.bar"] = new[]{ typeof(TestPackageSource)},
                ["disunity://disunity.io/api/v1"] = new []{ typeof(TestDisunityPackageSource),typeof(TestDisunity2PackageSource)}
            };
            
            var actual = _fixture.PackageSourceFactory.InstantiateSources(expectedSourceMap.Keys.ToArray());

            Assert.All(actual, source => {
                Assert.NotNull(source.SourceUri);
                var expectedSources = expectedSourceMap.GetValueOrDefault(source.SourceUri, null);
                Assert.NotNull(expectedSources);
                Assert.Contains(source.GetType(), expectedSources);
            });
        }

        [PackageSource("test://")]
        private class TestPackageSource : BaseTestPackageSource { }

        [PackageSource("disunity://")]
        private class TestDisunityPackageSource : BaseTestPackageSource { }

        [PackageSource("disunity://")]
        private class TestDisunity2PackageSource : BaseTestPackageSource { }

        private class BaseTestPackageSource : IPackageSource {

            public string SourceUri { get; set; }

            public Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier) {
                throw new NotImplementedException();
            }

            public Task<bool> CanHandlePackage(PackageIdentifier packageIdentifier) {
                throw new NotImplementedException();
            }

        }

    }

}