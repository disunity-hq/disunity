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

        public PackageIdentifier DisunityDistro { get; set; }

        public Dictionary<string, VersionRange> Mods { get; set; }

    }

    internal class ProfileMetaConfiguration : IEntityTypeConfiguration<ProfileMeta> {

        public void Configure(EntityTypeBuilder<ProfileMeta> builder) {
            builder.Property(p => p.Mods)
                   .HasJsonConversion()
                   .IsRequired();
        }

    }

}