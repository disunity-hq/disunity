using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class TargetVersionCompatibility: IVersionCompatibility<TargetVersion, UnityVersion> {
        
        public int ID { get; set; }

        [Required] public int VersionId { get; set; }
        public TargetVersion Version { get; set; }

        public int? MinCompatibleVersionId { get; set; }
        public UnityVersion MinCompatibleVersion { get; set; }

        public int? MaxCompatibleVersionId { get; set; }
        public UnityVersion MaxCompatibleVersion { get; set; }

        public class TargetVersionCompatibilityConfiguration: IEntityTypeConfiguration<TargetVersionCompatibility> {

            public void Configure(EntityTypeBuilder<TargetVersionCompatibility> builder) {
                // will need to remove unique constraint when adding disjoint ranges
                builder.HasIndex(c => c.VersionId).IsUnique();
            }

        }

    }

}