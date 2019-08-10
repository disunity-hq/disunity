using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModTargetCompatibility : IVersionCompatibility<ModVersion, TargetVersion> {

        public int VersionId { get; set; }
        public ModVersion Version { get; set; }

        public int TargetId { get; set; }
        public Target Target { get; set; }

        public int? MinCompatibleVersionId { get; set; }
        public TargetVersion MinCompatibleVersion { get; set; }

        public int? MaxCompatibleVersionId { get; set; }
        public TargetVersion MaxCompatibleVersion { get; set; }

        public class ModCompatibilityConfiguration : IEntityTypeConfiguration<ModTargetCompatibility> {

            public void Configure(EntityTypeBuilder<ModTargetCompatibility> builder) {
                builder.HasKey(c => new {VersionID = c.VersionId, TargetID = c.TargetId});
            }

        }

    }

}