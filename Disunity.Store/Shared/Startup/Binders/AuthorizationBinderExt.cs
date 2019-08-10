using System.Net;

using Disunity.Store.Policies;
using Disunity.Store.Policies.Requirements;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;


namespace Disunity.Store.Startup.Binders {

    public static class AuthorizationBinderExt {

        public static void ConfigureAuthorization(this IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("CanManageOrg", policy => policy.AddRequirements(new CanManageOrg()));
            });
            
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            services.ConfigureApplicationCookie(options => {
                options.Events.OnRedirectToLogin = async ctx => {
                    if (ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == (int) HttpStatusCode.OK) {
                        ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;

                        var body = JsonConvert.SerializeObject(new {
                            type = "Unauthorized",
                            error = "Not authorized.",
                        }, Formatting.Indented);

                        await ctx.Response.WriteAsync(body);
                    } else {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                };
            });
            
        }

    }

}