using Microsoft.AspNetCore.Authorization;


namespace Disunity.Store.Policies {

    public class OperationRequirement : IAuthorizationRequirement {

        public Operation Operation { get; }

        public OperationRequirement(Operation operation) {
            Operation = operation;
        }

    }

}