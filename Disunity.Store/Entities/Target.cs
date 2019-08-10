using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Entities
{

    public class Target
    {

        public int Id { get; set; }

        public int? LatestId { get; set; }

        public TargetVersion Latest { get; set; }
        
        [Required] [MaxLength(128)] public string Slug { get; set; }

        [InverseProperty("Target")] public List<TargetVersion> Versions { get; set; }

        public List<ModTargetCompatibility> Compatibilities { get; set; }

        public List<Mod> CompatibleMods => Compatibilities?.Select(c => c.Version.Mod).Distinct().ToList();


        [OnAfterCreate(typeof(TargetVersion))]
        public static void UpdateLatestVersion(TargetVersion newVersion, ApplicationDbContext context,
                                               IServiceProvider services)
        {
            var target = context.Targets.FirstOrDefault(t => t.Id == newVersion.TargetId);

            if (target == null)
            {
                return;
            }

            target.Latest = newVersion;
            context.SaveChanges();

        }

        public class TargetConfiguration : IEntityTypeConfiguration<Target>
        {

            public void Configure(EntityTypeBuilder<Target> builder)
            {
                builder.HasOne(t => t.Latest)
                       .WithOne(v => v.Target);

                builder.HasMany(t => t.Versions);

                builder.HasIndex(t => t.Slug).IsUnique();
            }

        }

    }

}