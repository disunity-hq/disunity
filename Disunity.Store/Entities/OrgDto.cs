using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities {

    public class OrgDto {

        /// <summary>
        /// The unique Id for this organization
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name to use when displaying this organization to the user
        /// </summary>
        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        /// <summary>
        /// A unique url slug to use when accessing endpoints for this organization 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// A list of all members within this organization and their roles
        /// </summary>
        public List<OrgMemberDto> Members { get; set; }

        /// <summary>
        /// A list of all mods owned by this organization
        /// </summary>
        public List<ModDto> Mods { get; set; }

    }

}