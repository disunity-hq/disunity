using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;


namespace Disunity.Store.Policies {

    public static class AuthorizationServiceExtensions {

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authService, ClaimsPrincipal user, object resource, Operation op) {
            return authService.AuthorizeAsync(user, resource, op.ToRequirement());
        }

    }

}