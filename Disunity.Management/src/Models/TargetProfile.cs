using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Management.Models {

    public class TargetProfile {

        public int TargetMetaId { get; set; }
        public TargetMeta TargetMeta { get; set; }

        public Guid ProfileMetaId { get; set; }
        public ProfileMeta ProfileMeta { get; set; }

    }

    internal class TargetProfileConfiguration : IEntityTypeConfiguration<TargetProfile> {

        public void Configure(EntityTypeBuilder<TargetProfile> builder) {

            builder.HasKey(tp => new {tp.ProfileMetaId, tp.TargetMetaId});
            
            builder.HasOne(tp => tp.ProfileMeta)
                   .WithMany(p => p.Targets)
                   .HasForeignKey(tp => tp.ProfileMetaId);

            builder.HasOne(tp => tp.TargetMeta)
                   .WithMany(t => t.Profiles)
                   .HasForeignKey(tp => tp.TargetMetaId);
        }

    }

}