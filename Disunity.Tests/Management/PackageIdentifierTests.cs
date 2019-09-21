using Disunity.Core;
using Disunity.Management.Models;

using Xunit;


namespace Disunity.Tests.Management {

    public class PackageIdentifierTests {

        [Fact]
        public void CanParseDisunityDistroIdentifier() {
            var id = new DisunityDistroIdentifier("1.0.0");

            Assert.True(id.Validate());

            Assert.Equal("1.0.0", id.Version);
        }

        [Fact]
        public void CanParseModIdentifier() {
            var id = new ModIdentifier("my-org", "my-mod", "1.0.0");

            Assert.True(id.Validate());

            Assert.Equal("my-org", id.OwnerSlug);
            Assert.Equal("my-mod", id.ModSlug);
            Assert.Equal("1.0.0", id.Version);
        }

    }

}