using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities {

    public class TargetDto {

        /// <summary>
        /// The unique ID of the taget
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The most recent version of this target
        /// </summary>
        public TargetVersionDto Latest { get; set; }

        /// <summary>
        /// The name used for display purposes to represent this target 
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// The URL slug at which this target is available
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Slug { get; set; }

        /// <summary>
        /// A list of all versions associated with this target
        /// </summary>
        public List<TargetVersionDto> Versions { get; set; }

    }

}