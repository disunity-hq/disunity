using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using Disunity.Management;
using Disunity.Management.Managers;
using Disunity.Management.Models;
using Disunity.Management.Services;
using Disunity.Management.Util;

using Moq;

using Newtonsoft.Json;

using Xunit;


namespace Disunity.Tests.Management {

    public class TargetFixture {


        public ITarget ExpectedTarget { get; }

        public IFileSystem MockFileSystem { get; }
        public ISymbolicLink MockSymbolicLink { get; }

        public TargetFixture() {
            var targetMeta = new TargetMeta {
                Slug = "risk-of-rain-2",
                DisplayName = "Risk of Rain 2",
                ExecutablePath = @"C:\Program Files\Risk of Rain 2\Risk of Rain.exe",
                ManagedPath = @"C:\test\managed\risk-of-rain-2_1492FF6C8FD37B8D9BC9120CEF7A8409",
            };

            ExpectedTarget = Target.CreateTarget(null)(targetMeta);

            var targetMetaPath = Path.Combine(ExpectedTarget.TargetMeta.ManagedPath, "target-info.json");
            var defaultProfilePath = Path.Combine(ExpectedTarget.TargetMeta.ManagedPath, "profiles", "default");

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                [defaultProfilePath] = new MockDirectoryData(),
                [targetMetaPath] = new MockFileData(JsonConvert.SerializeObject(ExpectedTarget))
            });


            MockSymbolicLink = Mock.Of<ISymbolicLink>();
        }

    }

    public class TargetTests : IClassFixture<TargetFixture> {

        private readonly TargetFixture _fixture;

        public TargetTests(TargetFixture fixture) {
            _fixture = fixture;
        }

        
//        [Fact]
//        public void CanDeleteTarget() {
//            Util.AssertDirectoryExists(_fixture.MockFileSystem, _fixture.ExpectedTarget.TargetMeta.ManagedPath);
//
//            _fixture.ExpectedTarget.Delete();
//            
//            Util.AssertDirectoryNotExists(_fixture.MockFileSystem, _fixture.ExpectedTarget.TargetMeta.ManagedPath);
//        }

    }

}