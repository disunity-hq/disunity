using System.ComponentModel.DataAnnotations;


namespace Disunity.Client.v1.Models {

    public enum OrgMemberRole {

        Owner,
        Admin,
        Member

    }
    
    public class OrgMemberDto {

        /// <summary>
        /// The id of a user that is in this organization
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The role this user has within this organization
        /// </summary>
        [Required]
        public OrgMemberRole Role { get; set; }

    }

}