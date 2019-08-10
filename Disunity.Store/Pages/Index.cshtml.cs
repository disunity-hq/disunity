using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages {

    [DefaultBreadcrumb("Home")]
    public class IndexModel : PageModel {

        public void OnGet() { }

    }

}