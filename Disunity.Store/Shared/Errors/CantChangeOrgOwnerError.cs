using System.Net;

using Disunity.Store.Entities;


namespace Disunity.Store.Errors {

    public class CantChangeOrgOwnerError : ApiError {

        public CantChangeOrgOwnerError(UserIdentity owner, UserIdentity rejectedUser) 
            : base($"{owner.UserName} cannot be replaced by {rejectedUser.UserName} as the owner of their own Org.") {
        }

    }

}