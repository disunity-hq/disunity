using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

using Disunity.Management;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests.Management {

    public class HelperTestFixture {

        public Crypto Crypto { get; }

        public HelperTestFixture() {
            var services = new ServiceCollection();
            services.Configure<CryptoOptions>(options => { });
            services.AddSingleton<Crypto>();
            Crypto = services.BuildServiceProvider().GetRequiredService<Crypto>();
        }

    }

    public class HelperTests : IClassFixture<HelperTestFixture> {

        private const string ManagedRoot = @"C:\test\managed";

        private Crypto Crypto { get; }

        public HelperTests(HelperTestFixture fixture) {
            Crypto = fixture.Crypto;
        }

        private Target _target = new Target(new TargetMeta {
            Slug = "risk-of-rain-2",
            DisplayName = "Risk of Rain 2",
            ExecutablePath = @"C:\Program Files\Risk of Rain 2\Risk of Rain.exe",
        });

    [Fact]
        public void CanCreateProperManagedTargetPath_FullHashLength() {

            var hash = Crypto.HashString(_target.TargetMeta.ExecutablePath);
            var expected = $"{_target.TargetMeta.Slug}_{hash}";
            var actual = Crypto.CalculateManagedPath(_target);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(1)]
        public void CanCreateProperManagedTargetPath_ShortHashLength(int hashSize) {

            var hash = Crypto.HashString(_target.TargetMeta.ExecutablePath);
            var actual = Crypto.CalculateManagedPath(_target, hashSize);
            var expected = $"{_target.TargetMeta.Slug}_{hash.Substring(0, hashSize * 2)}";
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void CanCreatProperManagedTargetPath_NoHash() {
            var actual = Crypto.CalculateManagedPath(_target, 0);

            Assert.Equal(_target.TargetMeta.Slug, actual);
        }

        [Fact]
        public void CanSerializeMod() {
            var mod = new Mod {
                Name = "foo",
                Owner = "bar"
            };

            var json = TypeDescriptor.GetConverter(mod).ConvertToString(mod);

            var expected = "bar/foo";

            Assert.Equal(expected, json);
        }

        [Fact]
        public void CanDeserializeMod() {
            var expected = new Mod {
                Name = "foo",
                Owner = "bar"
            };

            var json = "bar/foo";

            var actual = (Mod) TypeDescriptor.GetConverter(expected).ConvertFromString(json);

            Assert.Equal(expected, actual);
        }

    }

}