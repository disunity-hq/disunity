using Disunity.Store.Entities;


namespace Disunity.Store.Errors {

    public class DuplicateOrgOwnerError : ApiError {

        public DuplicateOrgOwnerError(UserIdentity rejectedUser, Org org)
            : base($"User {rejectedUser.UserName} can not replace {org.Slug} as the owner of the {org.Slug} org.") { }
    }

}