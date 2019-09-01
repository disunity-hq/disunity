using System.Collections.Generic;


namespace Disunity.Management.Models {

    public class TargetMeta {

        /// <summary>
        /// Internal Id for this target. Used mostly for linking references
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Absolute path to the target executable
        /// </summary>
        public string ExecutablePath { get; set; }
        
        /// <summary>
        /// The most recent hash calculated by the management layer for the target
        /// </summary>
        /// <remarks>
        /// Used mostly for detecting and validating target versions
        /// </remarks>
        public string LatestHash { get; set; }

        /// <summary>
        /// Relative path from the root of managed target dirs to this target's dir
        /// </summary>
        public string ManagedPath { get; set; }

        /// <summary>
        /// The unique slug for the target to use when looking up data from disunity.io
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Display name to use for this target when showing to the user. Defaults to name of target as reported by disunity.io
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The currently active profile on this target
        /// </summary>
        public ProfileMeta ActiveProfile { get; set; }

        /// <summary>
        /// All profiles associated with this target
        /// </summary>
        public List<TargetProfile> Profiles { get; set; }

    }

}