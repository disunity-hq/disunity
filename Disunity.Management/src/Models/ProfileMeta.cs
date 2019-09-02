using System;
using System.Collections.Generic;

using Disunity.Core.Archives;
using Disunity.Management.Extensions;
using Disunity.Management.PackageStores;
using Disunity.Management.Util;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Management.Models {

    public class ProfileMeta {

        /// <summary>
        /// Internal id of this profile.
        /// </summary>
        /// <remarks>
        /// Used to generate a unique directory for each profile
        /// </remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// A list of all targets that are using this profile, either active or in-active
        /// </summary>
        public List<TargetProfile> Targets { get; set; }

        /// <summary>
        /// The disunity distro installed into this profile
        /// </summary>
        public DisunityDistroIdentifier DisunityDistro { get; set; }

        /// <summary>
        /// The mod requirements of this profile
        /// </summary>
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