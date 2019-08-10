namespace Disunity.Store.Entities {

    public class ModDependencyDto {

        /// <summary>
        /// The mod version id whose dependency is being described
        /// </summary>
        public int DependantId { get; set; }

        /// <summary>
        /// The dependency id required by this mod
        /// </summary>
        public int DependencyId { get; set; }

        /// <summary>
        /// The min version compatible with this mod
        /// May be null, signifying all versions below max version are compatible
        /// </summary>
        public int? MinVersionId { get; set; }

        /// <summary>
        /// The max version compatible with this mod.
        /// May be null, signifying all versions above min version are compatible
        /// </summary>
        public int? MaxVersionId { get; set; }

    }

}