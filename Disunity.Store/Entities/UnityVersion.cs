using System.ComponentModel.DataAnnotations;

using Disunity.Store.Data;
using Disunity.Store.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class UnityVersion: IVersionModel {

        public int Id { get; set; }

        public int VersionNumberId { get; set; }
        public VersionNumber VersionNumber { get; set; }

        public class UnityVersionConfiguration : IEntityTypeConfiguration<UnityVersion> {

            public void Configure(EntityTypeBuilder<UnityVersion> builder) {
                builder.HasVersionIndex().IsUnique();
            }

        }

    }

}