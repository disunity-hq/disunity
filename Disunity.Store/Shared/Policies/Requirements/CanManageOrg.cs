using Microsoft.AspNetCore.Authorization;


namespace Disunity.Store.Policies.Requirements {

    public class CanManageOrg : IAuthorizationRequirement {

        public CanManageOrg() { }

    }

}