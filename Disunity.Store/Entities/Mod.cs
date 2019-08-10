using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Disunity.Store.Data;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class Mod : ICreatedAt {

        public int Id { get; set; }

        public int? OwnerId { get; set; }
        public Org Owner { get; set; }

        [Required] [MaxLength(128)] public string Slug { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeprecated { get; set; }
        public bool? IsPinned { get; set; }

        public int? LatestId { get; set; }
        public ModVersion Latest { get; set; }

        [InverseProperty("Mod")] public List<ModVersion> Versions { get; set; }

        [NotMapped] public int TotalDownloads => Versions.Select(v => v.Downloads).Sum() ?? 0;

        [OnAfterCreate(typeof(ModVersion))]
        public static void OnBeforeCreateModVersion(ModVersion entity, ApplicationDbContext context) {
            var mod = context.Mods.FirstOrDefault(m => m.Id == entity.ModId);

            if (mod == null) {
                return;
            }

            mod.Latest = entity;
            context.SaveChanges();
        }

        public class ModConfiguration : IEntityTypeConfiguration<Mod> {

            public void Configure(EntityTypeBuilder<Mod> builder) {
                // Set field defaults
                builder.Property(m => m.IsActive).HasDefaultValue(true);
                builder.Property(m => m.IsDeprecated).HasDefaultValue(false);
                builder.Property(m => m.IsPinned).HasDefaultValue(false);

                // Ensure slug is unique within the org
                builder.HasAlternateKey(m => new {m.OwnerId, m.Slug});

                builder
                    .HasOne(m => m.Owner)
                    .WithMany(o => o.Mods)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasOne(m => m.Latest);

                builder
                    .HasMany(m => m.Versions)
                    .WithOne(v => v.Mod);
            }

        }

        public DateTime CreatedAt { get; set; }

    }

}