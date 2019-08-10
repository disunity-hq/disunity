using Disunity.Store.Entities;

using Microsoft.AspNetCore.Authorization;


namespace Disunity.Store.Policies.Requirements {

    public class HasOrgRole : IAuthorizationRequirement {

        public OrgMemberRole Role;
    
        public HasOrgRole(OrgMemberRole role) {
            Role = role;
        }

    }

}