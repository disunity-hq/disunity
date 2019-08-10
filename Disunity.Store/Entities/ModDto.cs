using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities {

    public class ModDto {

        /// <summary>
        /// Unique Id of this mod
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Organization that owns this mod
        /// </summary>
        public OrgDto Owner { get; set; }

        /// <summary>
        /// The name to be used for display purposes
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// The unique slug used to access this mod's endpoints
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Slug { get; set; }

        /// <summary>
        /// Whether or not this mod is active (inactive mods are hidden on site)
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Whether or not this mod should be flagged as deprecated
        /// </summary>
        public bool? IsDeprecated { get; set; }

        /// <summary>
        ///  Whether or not this mod should be pinned to the top of the mod list within a target
        /// </summary>
        public bool? IsPinned { get; set; }

        /// <summary>
        /// Details on the most recent version of the mod
        /// </summary>
        public ModVersionDto Latest { get; set; }

        /// <summary>
        /// A list of all versions for this mod
        /// </summary>
        public List<ModVersionDto> Versions { get; set; }
        
        /// <summary>
        /// The UTC time at which this mod was first uploaded
        /// </summary>
        public DateTime CreatedAt { get; set; }

    }

}