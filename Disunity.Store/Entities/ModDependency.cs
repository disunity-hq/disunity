using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public enum ModDependencyType {

        /// <summary>
        /// This mod is a normal dependency 
        /// </summary>
        Dependency,
        /// <summary>
        /// 
        /// </summary>
        OptionalDependency,
        Incompatible

    }

    public class ModDependency {

        public int DependentId { get; set; }
        /// <summary>
        /// The <see cref="ModVersion"/> whose dependency is being described
        /// </summary>
        public ModVersion Dependent { get; set; }

        public int DependencyId { get; set; }
        /// <summary>
        /// The dependency required by <see cref="Dependent"/>
        /// </summary>
        public Mod Dependency { get; set; }

        /// <summary>
        /// The type of dependency represented by this row
        /// </summary>
        public ModDependencyType DependencyType { get; set; }

        public int? MinVersionId { get; set; }
        /// <summary>
        /// The min version compatible with <see cref="Dependent"/>.
        /// May be null, signifying all versions below <see cref="MaxVersion"/> are compatible
        /// </summary>
        public ModVersion MinVersion { get; set; }

        public int? MaxVersionId { get; set; }
        /// <summary>
        /// The max version compatible with <see cref="Dependent"/>.
        /// May be null, signifying all versions above <see cref="MinVersion"/> are compatible
        /// </summary>
        public ModVersion MaxVersion { get; set; }
        
        public class ModDependencyConfiguration: IEntityTypeConfiguration<ModDependency> {

            public void Configure(EntityTypeBuilder<ModDependency> builder) {
                builder.HasKey(c => new {DependantId = c.DependentId, c.DependencyId});

                builder.HasOne(d => d.MinVersion);
                builder.HasOne(d => d.MaxVersion);
            }

        }

    }

}