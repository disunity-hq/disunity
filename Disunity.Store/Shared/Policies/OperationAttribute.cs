using System;
using System.Linq;
using System.Reflection;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Policies {

    public abstract class OperationAttribute : Attribute {

        public readonly Operation Operation;
        public readonly string[] RouteValues;
        public readonly bool API;

        protected OperationAttribute(Operation operation, string[] routeValues, bool api = true) {
            RouteValues = routeValues;
            Operation = operation;
            API = api;
        }

        protected abstract object GetResource(AuthorizationFilterContext context, object[] routeValues);

        private object[] GetRouteValues(RouteValueDictionary values) {
            return RouteValues.Select(key => values.ContainsKey(key) ? values[key] : null).ToArray();
        }

        public object GetResource(AuthorizationFilterContext context) {
            var routeValues = GetRouteValues(context.RouteData.Values);
            return GetResource(context, routeValues);
        }

    }

    public class OrgOperationAttribute : OperationAttribute {

        public OrgOperationAttribute(Operation operation, string routeValues) :
            base(operation, new[] {routeValues}) { }

        protected override object GetResource(AuthorizationFilterContext context, object[] routeValues) {
            var orgSlug = (string) routeValues[0];
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            return dbContext.Orgs.SingleOrDefault(o => o.Slug == orgSlug);
        }

    }

    public class ModOperationAttribute : OperationAttribute {

        public ModOperationAttribute(Operation operation, string orgSlug = "orgSlug", string modSlug = "modSlug") :
            base(operation, new[] {orgSlug, modSlug}) { }

        protected override object GetResource(AuthorizationFilterContext context, object[] routeValues) {
            var orgSlug = routeValues[0] as string;
            var modSlug = routeValues[1] as string;
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            return dbContext.Mods.SingleOrDefault(m => m.Slug == modSlug && m.Owner.Slug == orgSlug);
        }

    }

}