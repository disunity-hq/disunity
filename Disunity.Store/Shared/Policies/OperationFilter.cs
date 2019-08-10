using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Policies {

    public class OperationFilter : IAuthorizationFilter {

        private static void ProcessMethodAttribute(ILogger<OperationFilter> logger,
                                                   AuthorizationFilterContext context,
                                                   IAuthorizationService authService,
                                                   OperationAttribute attr) {
            var resource = attr.GetResource(context);

            if (resource == null) {
                logger.LogDebug("Resource was null.");
                context.Result = new NotFoundResult();
                return;
            }

            var authorization = authService.AuthorizeAsync(context.HttpContext.User, resource, attr.Operation).Result;

            logger.LogDebug($"Authorization succeeded: {authorization.Succeeded}");

            if (!authorization.Succeeded) {
                context.Result = attr.API ? (IActionResult) new StatusCodeResult(403) : new ForbidResult();
            }
        }

        private static void ProcessAttributes(ILogger<OperationFilter> logger,
                                              AuthorizationFilterContext context,
                                              IAuthorizationService authService,
                                              IEnumerable<OperationAttribute> attributes) {
            foreach (var attr in attributes) {
                ProcessMethodAttribute(logger, context, authService, attr);
            }

        }

        private static IEnumerable<OperationAttribute> AttrsFromController(ActionDescriptor descriptor) {
            var methodInfo = descriptor is ControllerActionDescriptor controllerDescriptor
                ? controllerDescriptor.MethodInfo
                : null;

            if (methodInfo == null) {
                return new OperationAttribute[] { };
            }

            var attrs = methodInfo.GetCustomAttributes(typeof(OperationAttribute), true);
            return attrs.Select(a => (OperationAttribute) a);
        }


        public void OnAuthorization(AuthorizationFilterContext context) {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OperationFilter>>();
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var attrs = AttrsFromController(context.ActionDescriptor);
            ProcessAttributes(logger, context, authService, attrs);
        }

    }

}