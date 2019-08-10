namespace Disunity.Store.Entities {

    public class DisunityVersionDto {

        /// <summary>
        /// The download url this disunity distro can be downlaoded from
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The version number of this disunity distro
        /// </summary>
        public string VersionNumber { get; set; }

        /// <summary>
        /// The minimum unity version compatible with this version of disunity.
        /// Null for no minimum
        /// </summary>
        public string MinUnityVersion { get; set; }

        /// <summary>
        /// The maximum unity version compatible with this version of disunity.
        /// Null for no maximum
        /// </summary>
        public string MaxUnityVersion { get; set; }

    }

}