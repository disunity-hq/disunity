using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Disunity.Management.Managers;
using Disunity.Management.PackageStores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Management.Models {

    public class Package : IPackage {

        private readonly HashSet<ProfileMeta> _references;

        public Package() {
            _references = new HashSet<ProfileMeta>();
        }

        [NotMapped]
        public PackageIdentifier Id { get; set; }

        public IReadOnlyCollection<ProfileMeta> References => _references;

//        public IPackageStore Source { get; set; }

        public void CreateReference(ProfileMeta profile) {
            throw new System.NotImplementedException();
        }

        public void RemoveReference(ProfileMeta profile) {
            throw new System.NotImplementedException();
        }

    }

    internal class PackageConfiguration : IEntityTypeConfiguration<Package> {

        public void Configure(EntityTypeBuilder<Package> builder) {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasConversion(
                       s => s.Id,
                       d => new PackageIdentifier(d));
        }

    }

}