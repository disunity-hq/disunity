using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Disunity.Core.Archives;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests {
    

    public class ManifestFixture {

        public ManifestFixture() {
            Json = File.ReadAllText("data/manifest.json");

            Factory = json => {
                Manifest.ValidateJson(json);
                return JsonConvert.DeserializeObject<Manifest>(json);
            };
        }

        public string Json { get; }
        public Func<string, Manifest> Factory { get; }

    }

    public class ManifestTests : IClassFixture<ManifestFixture> {

        public ManifestTests(ITestOutputHelper log, ManifestFixture fixture) {
            this.log = log;
            this.fixture = fixture;
        }

        private readonly ITestOutputHelper log;

        private readonly ManifestFixture fixture;

        public bool RangesAreEqual(Dictionary<string, VersionRange> first,
                                   Dictionary<string, VersionRange> second) {
            if (first.Count != second.Count) {
                log.WriteLine($"Count is different: {first.Count} vs {second.Count}");
                return false;
            }

            foreach (var (key, value) in first.Select(x => (x.Key, x.Value))) {
                if (!second.TryGetValue(key, out var range)) {
                    log.WriteLine($"Missing key: {key}");
                    return false;
                }

                if (!range.Equals(value)) {
                    log.WriteLine($"Versions are not equal: {value} vs {range}");
                    return false;
                }
            }

            return true;
        }

        [Fact]
        public void CanDeserialize() {
            var manifest = fixture.Factory(fixture.Json);

            foreach (var key in manifest.Targets.Keys) {
                var target = manifest.Targets[key];
                log.WriteLine($"Dependency: {key} - {target.MinVersion} to {target.MaxVersion}");
            }

            Assert.Equal("fake-user", manifest.OrgID);
            Assert.Equal("fake-mod", manifest.ModID);
            Assert.Equal("0.0.1", manifest.Version);
            Assert.Equal("Fake Mod", manifest.DisplayName);
            Assert.Equal("http://github.com/disunity-hq/fake-mod", manifest.URL);
            Assert.Equal("An imaginary Disunity mod", manifest.Description);
            Assert.Equal(14, manifest.ContentTypes);

            Assert.True(RangesAreEqual(new Dictionary<string, VersionRange> {
                {"risk-of-rain-2", new VersionRange("3830295", "3830297")}
            }, manifest.Targets));

            Assert.True(RangesAreEqual(new Dictionary<string, VersionRange> {
                {"foo/bar", new VersionRange("2.0.0")}
            }, manifest.Dependencies));

            Assert.Equal(new[] {"FakePreload.dll"}, manifest.PreloadAssemblies);
            Assert.Equal(new[] {"FakeRuntime.dll"}, manifest.RuntimeAssemblies);

            Assert.Equal("FakePreload", manifest.PreloadAssembly);
            Assert.Equal("FakePreload", manifest.PreloadClass);
            Assert.Equal("FakeRuntime", manifest.RuntimeAssembly);
            Assert.Equal("FakeRuntime", manifest.RuntimeClass);
        }

    }

}
