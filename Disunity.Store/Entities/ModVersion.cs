using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Disunity.Store.Data;
using Disunity.Store.Extensions;
using Disunity.Store.Storage;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities {

    public partial class ModVersion : ICreatedAt, IVersionModel {

        public int Id { get; set; }

        [Required] public int ModId { get; set; }
        public Mod Mod { get; set; }

        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        public bool? IsActive { get; set; }
        public int? Downloads { get; set; }

        public int VersionNumberId { get; set; }
        public VersionNumber VersionNumber { get; set; }

        [Required] [MaxLength(1024)] public string WebsiteUrl { get; set; }

        [Required] [MaxLength(256)] public string Description { get; set; }

        [Required] [MaxLength] public string Readme { get; set; }

        [MaxLength(1024)] [Required] public string FileId { get; set; }

        [MaxLength(1024)]
        [Required]
        [DataType(DataType.ImageUrl)]
        public string IconUrl { get; set; }

        [InverseProperty("Dependent")] public List<ModDependency> ModDependencies { get; set; }
        [InverseProperty("Version")] public List<ModTargetCompatibility> TargetCompatibilities { get; set; }
        [InverseProperty("Version")] public List<ModDisunityCompatibility> DisunityCompatibilities { get; set; }

        [NotMapped]
        public List<ModDependency> Dependencies =>
            ModDependencies.Where(d => d.DependencyType == ModDependencyType.Dependency).ToList();

        [NotMapped]
        public List<ModDependency> OptionalDependencies => ModDependencies
                                                           .Where(d => d.DependencyType ==
                                                                       ModDependencyType.OptionalDependency).ToList();

        [NotMapped]
        public List<ModDependency> Incompatibilities =>
            ModDependencies.Where(d => d.DependencyType == ModDependencyType.Incompatible).ToList();

        public DateTime CreatedAt { get; set; }

    }

    public partial class ModVersion {

        [OnAfterDelete]
        public static void CleanupStorageProvider(ModVersion modVersion, IServiceProvider services) {
            using (var scope = services.CreateScope()) {
                var storageProvider = scope.ServiceProvider.GetRequiredService<IStorageProvider>();
                storageProvider.DeleteFile(modVersion.FileId).Wait();
            }
        }

        public class ModVersionConfiguration : IEntityTypeConfiguration<ModVersion> {

            public void Configure(EntityTypeBuilder<ModVersion> builder) {
                builder.Property(v => v.Downloads).HasDefaultValue(0);
                builder.Property(v => v.IsActive).HasDefaultValue(false);

                builder.HasIndex(v => new {v.ModId, v.VersionNumberId}).IsUnique();

                builder.HasMany(v => v.ModDependencies).WithOne(d => d.Dependent);
                builder.HasMany(v => v.TargetCompatibilities).WithOne(c => c.Version);

            }

        }

    }

}