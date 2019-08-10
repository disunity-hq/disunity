using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages {

    [Breadcrumb("Privacy Policy", FromPage = typeof(IndexModel))]
    public class PrivacyModel : PageModel {

        public void OnGet() { }

    }

}