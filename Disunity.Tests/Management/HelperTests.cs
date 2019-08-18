using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using Disunity.Management;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Tests.Management {

    public class HelperTests {

        private const string ManagedRoot = @"C:\test\managed";
        private const string RiskOfRainHash = "1492FF6C8FD37B8D9BC9120CEF7A8409";

        private Target _target = new Target {
            Slug = "risk-of-rain-2",
            DisplayName = "Risk of Rain 2",
            ExecutableName = "Risk of Rain.exe",
            TargetPath = @"C:\Program Files\Risk of Rain 2"
        };

        [Fact]
        public void CanCreateProperManagedTargetPath_FullHashLength() {

            var actual = Crypto.CalculateManagedPath(_target);
            var expected = $"{_target.Slug}_{RiskOfRainHash}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateProperManagedTargetPath_ShortHashLength() {
            const int hashSize = 4;

            var actual = Crypto.CalculateManagedPath(_target, hashSize);
            var expected = $"{_target.Slug}_{RiskOfRainHash.Substring(0, hashSize * 2)}";

            Assert.Equal(expected, actual);
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

            var actual = (Mod)TypeDescriptor.GetConverter(expected).ConvertFromString(json); 

            Assert.Equal(expected, actual);
        }

    }

}