using Disunity.Store.Entities;


namespace Disunity.Store.Errors {

    public class DuplicateOrgMemberError : ApiError {

        public DuplicateOrgMemberError(UserIdentity user, Org org)
            : base($"The user {user.UserName} is already a member of the {org.Slug} org.") { }

    }

}