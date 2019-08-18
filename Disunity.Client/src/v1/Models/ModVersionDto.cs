using System;
using System.Collections.Generic;

namespace Disunity.Client.v1.Models {

    public class ModVersionDto {

        /// <summary>
        /// The unique Id for this mod version
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A reference to the unique id of the mod this version belongs to
        /// </summary>
        public int ModId { get; set; }

        /// <summary>
        /// The name to use when displaying this version to the users
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Flag indicating whether or not this mod is actively shown on the Disunity.io store
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// The total downloads counted for this version
        /// </summary>
        public int? Downloads { get; set; }

        /// <summary>
        /// The version number used by this mod version. Follows the MAJOR.MINOR.PATCH pattern
        /// </summary>
       public string VersionNumber { get; set; }

        /// <summary>
        /// A link to the mod's website for this version
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        ///  A brief description of the mod
        /// </summary>
       public string Description { get; set; }

        /// <summary>
        /// The readme markdown associated with this mod version
        /// </summary>
        public string Readme { get; set; }

        /// <summary>
        /// A link to the archive file used to install this version of the mod
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// A link to an image to be used when displaying an icon for this mod
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// A list of all dependencies this mod requires
        /// </summary>
        public List<ModDependencyDto> Dependencies { get; set; }
        
        /// <summary>
        /// A list of all target compatibilites this mod has
        /// </summary>
        public List<ModTargetCompatibilityDto> TargetCompatibilities { get; set; }


        /// <summary>
        /// The UTC time at which this version was uploaded
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

}