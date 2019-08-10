using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class DisunityVersionCompatibility:IVersionCompatibility<DisunityVersion, UnityVersion> {

        public int ID { get; set; }

        [Required] public int VersionId { get; set; }
        public DisunityVersion Version { get; set; }

        public int? MinCompatibleVersionId { get; set; }
        public UnityVersion MinCompatibleVersion { get; set; }

        public int? MaxCompatibleVersionId { get; set; }
        public UnityVersion MaxCompatibleVersion { get; set; }


        public class
            DisunityVersionCompatibilityConfiguration : IEntityTypeConfiguration<DisunityVersionCompatibility> {

            public void Configure(EntityTypeBuilder<DisunityVersionCompatibility> builder) {
                // will need to remove unique constraint when adding disjoint ranges
                builder.HasIndex(c => c.VersionId).IsUnique();

            }

        }

    }

}