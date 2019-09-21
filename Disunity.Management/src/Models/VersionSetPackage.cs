using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Management.Models {

    public class VersionSetPackage {

        public int VersionSetId { get; set; }
        public VersionSet VersionSet { get; set; }
        
        public PackageIdentifier PackageId { get; set; }
        public Package Package { get; set; }

    }
    
    internal class VersionSetPackageConfiguration: IEntityTypeConfiguration<VersionSetPackage> {

        public void Configure(EntityTypeBuilder<VersionSetPackage> builder) {
            builder.HasKey(m => new {m.VersionSetId, m.PackageId});

#pragma warning disable 618
            builder.HasOne(m => m.VersionSet)
                   .WithMany(v => v.VersionSetPackages)
                   .HasForeignKey(m => m.VersionSetId);
#pragma warning restore 618

            builder.HasOne(m => m.Package)
                   .WithMany()
                   .HasForeignKey(m => m.PackageId);

            builder.Property(p => p.PackageId)
                   .HasConversion(
                       s => s.Id,
                       d => new PackageIdentifier(d));
        }

    }

}