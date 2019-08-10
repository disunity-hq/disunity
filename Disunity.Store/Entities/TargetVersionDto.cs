using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities {

    public class TargetVersionDto {

        /// <summary>
        /// The name to use when displaying this version to the user 
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// The version number associated with this <see cref="TargetVersionDto"/>
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string VersionNumber { get; set; }

        /// <summary>
        /// The minimum unity version compatible with this target version
        /// </summary>
        [Required]
        public string MinCompatibleVersion { get; set; }

        /// <summary>
        /// The maximum unity version compatible with this target version
        /// </summary>
        [Required]
        public string MaxCompatibleVersion { get; set; }

        /// <summary>
        /// The target's website
        /// </summary>
        [Required]
        [MaxLength(1024)]
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// A brief description of the target
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// A Url to an image to be used as an icon when representing this target
        /// </summary>
        [Required]
        [MaxLength(1024)]
        [DataType(DataType.ImageUrl)]
        public string IconUrl { get; set; }

        /// <summary>
        /// A SHA-256 hash of the target executable. Used during auto-discovery of targets
        /// </summary>
        [MaxLength(128)]
        public string Hash { get; set; }

    }

}