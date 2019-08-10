using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModDisunityCompatibility:IVersionCompatibility<ModVersion,DisunityVersion> {

        public int ID { get; set; }

        public int VersionId { get; set; }
        public ModVersion Version { get; set; }
        
        public int? MinCompatibleVersionId { get; set; }
        public DisunityVersion MinCompatibleVersion { get; set; }
        
        public int? MaxCompatibleVersionId { get; set; }
        public DisunityVersion MaxCompatibleVersion { get; set; }

        public class ModDisunityCompatibilityConfiguration: IEntityTypeConfiguration<ModDisunityCompatibility> {

            public void Configure(EntityTypeBuilder<ModDisunityCompatibility> builder) {
                builder.HasIndex(c => c.VersionId).IsUnique();
            }

        }

    }

}