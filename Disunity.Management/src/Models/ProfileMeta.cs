using System;
using System.Collections.Generic;

using Disunity.Core.Archives;
using Disunity.Management.PackageStores;
using Disunity.Management.Util;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Management.Models {

    public class ProfileMeta {

        public Guid Id { get; set; }

        public List<TargetProfile> Targets { get; set; }

        public DisunityDistroIdentifier DisunityDistro { get; set; }

        public Dictionary<ModIdentifier, VersionRange> Mods { get; set; }

    }

    internal class ProfileMetaConfiguration : IEntityTypeConfiguration<ProfileMeta> {

        public void Configure(EntityTypeBuilder<ProfileMeta> builder) {
            builder.Property(p => p.Mods)
                   .HasJsonConversion()
                   .IsRequired();
        }

    }

}