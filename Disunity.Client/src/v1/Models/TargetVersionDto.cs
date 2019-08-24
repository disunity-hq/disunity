

namespace Disunity.Client.v1.Models {

    public class TargetVersionDto {

        /// <summary>
        /// The name to use when displaying this version to the user 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The version number associated with this <see cref="TargetVersionDto"/>
        /// </summary>
        public string VersionNumber { get; set; }

        /// <summary>
        /// The minimum unity version compatible with this target version
        /// </summary>
        public string MinCompatibleVersion { get; set; }

        /// <summary>
        /// The maximum unity version compatible with this target version
        /// </summary>
        public string MaxCompatibleVersion { get; set; }

        /// <summary>
        /// The target's website
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// A brief description of the target
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A Url to an image to be used as an icon when representing this target
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// A SHA-256 hash of the target executable. Used during auto-discovery of targets
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// The unique slug attached to the target
        /// </summary>
        public string Slug { get; set; }

    }

}