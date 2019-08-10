using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Startup.Filters {

    public class BreadcrumbFilter : IAsyncPageFilter {

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                      PageHandlerExecutionDelegate next) {
            var page = context.HandlerInstance as PageModel;

            if (page == null) {
                return;
            }

            var pageType = context.ActionDescriptor.DeclaredModelTypeInfo;
            var attrs = pageType.GetCustomAttributes(typeof(BreadcrumbAttribute));

            if (attrs.Any() && attrs.First() is BreadcrumbAttribute breadcrumbAttribute) {
                page.ViewData["ShowBreadcrumbs"] = !breadcrumbAttribute.Default;
            }

            await next();
        }

    }

}