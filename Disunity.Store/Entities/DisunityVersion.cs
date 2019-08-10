using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Disunity.Store.Data;
using Disunity.Store.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class DisunityVersion : IVersionModel {

        public int Id { get; set; }

        [DataType(DataType.Url)] public string Url { get; set; }

        [Required] public int VersionNumberId { get; set; }

        public VersionNumber VersionNumber { get; set; }
        
        [InverseProperty("Version")] public DisunityVersionCompatibility CompatibleUnityVersion { get; set; }

        public class DisunityVersionConfiguration : IEntityTypeConfiguration<DisunityVersion> {

            public void Configure(EntityTypeBuilder<DisunityVersion> builder) {
                builder.HasVersionIndex().IsUnique();
            }

        }

    }

}