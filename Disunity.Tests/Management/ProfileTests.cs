using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using Disunity.Management;
using Disunity.Management.Managers;
using Disunity.Management.Models;
using Disunity.Management.Util;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class ProfileFixture {

        public ITarget ExpectedTarget { get; }


        public ProfileFixture() {
            var targetMeta = new TargetMeta {
                Slug = "risk-of-rain-2",
                DisplayName = "Risk of Rain 2",
                ExecutablePath = @"C:\Program Files\Risk of Rain 2\Risk of Rain.exe",
                ManagedPath = @"C:\test\managed\risk-of-rain-2_1492FF6C8FD37B8D9BC9120CEF7A8409",
            };

            ExpectedTarget = Target.CreateTarget(null)(targetMeta);


        }

    }

    public class ProfileTests : IClassFixture<ProfileFixture> {

        private readonly ProfileFixture _fixture;

        public ProfileTests(ProfileFixture fixture) {
            _fixture = fixture;
        }


    }

}