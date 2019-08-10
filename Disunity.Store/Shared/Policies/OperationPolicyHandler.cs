using System.Reflection;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Authorization;


namespace Disunity.Store.Policies {

    public abstract class OperationPolicyHandler<TResource> : AuthorizationHandler<OperationRequirement, TResource> {

        protected bool CheckSuperUser(AuthorizationHandlerContext context, OperationRequirement requirement) {
            if (!context.User.IsInRole(UserRoles.Admin.ToString())) {
                return false;
            }

            context.Succeed(requirement);
            return true;

        }

        protected MethodInfo GetHandler(OperationRequirement requirement) {
            var name = $"{requirement.Operation.ToString()}Op";
            var type = GetType();
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }

    }

}