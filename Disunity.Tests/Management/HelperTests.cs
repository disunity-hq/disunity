using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

using Disunity.Management;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests.Management {

    public class HelperTests {

        private const string ManagedRoot = @"C:\test\managed";

        private Target _target = new Target {
            Slug = "risk-of-rain-2",
            DisplayName = "Risk of Rain 2",
            ExecutableName = "Risk of Rain.exe",
            TargetPath = @"C:\Program Files\Risk of Rain 2"
        };

        [Fact]
        public void CanCreateProperManagedTargetPath_FullHashLength() {

            using (var md5 = MD5.Create()) {
                var hash = Crypto.HashString(_target.ExecutablePath, md5);
                var expected = $"{_target.Slug}_{hash}";
                var actual = Crypto.CalculateManagedPath(_target);

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(1)]
        public void CanCreateProperManagedTargetPath_ShortHashLength(int hashSize) {

            using (var md5 = MD5.Create()) {
                var hash = Crypto.HashString(_target.ExecutablePath, md5);
                var actual = Crypto.CalculateManagedPath(_target, hashSize);
                var expected = $"{_target.Slug}_{hash.Substring(0, hashSize * 2)}";
                Assert.Equal(expected, actual);
            }

        }

        [Fact]
        public void CanCreatProperManagedTargetPath_NoHash() {
            var actual = Crypto.CalculateManagedPath(_target, 0);

            Assert.Equal(_target.Slug, actual);
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