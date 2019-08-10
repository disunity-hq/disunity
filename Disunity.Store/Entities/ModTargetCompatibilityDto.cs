namespace Disunity.Store.Entities {

    public class ModTargetCompatibilityDto {

        /// <summary>
        /// The mod version id
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// The id of a target this mod is compatible with
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// The id of the minimum target version. Null for no minimum
        /// </summary>
        public int? MinCompatibleVersionId { get; set; }

        /// <summary>
        /// The id of the maximum target version. Null for no maximum
        /// </summary>
        public int? MaxCompatibleVersionId { get; set; }

    }

}