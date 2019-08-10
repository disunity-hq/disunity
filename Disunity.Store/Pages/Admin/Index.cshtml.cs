using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin {

    [Breadcrumb("Admin", FromPage = typeof(Disunity.Store.Pages.IndexModel))]
    public class IndexModel : PageModel {

        public void OnGet() { }

    }

}