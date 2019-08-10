using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Disunity.Store.Policies {

    public static class OperationsExtensions {

        public static OperationRequirement ToRequirement(this Operation op) {
            return new OperationRequirement(op);
        }

    }

}