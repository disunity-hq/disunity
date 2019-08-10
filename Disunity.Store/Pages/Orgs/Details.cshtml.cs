using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;


namespace Disunity.Store.Pages.Orgs {

    [Breadcrumb]
    public class Details : PageModel {

        private readonly ILogger<Details> _logger;
        private readonly ApplicationDbContext _context;
        public Details(ApplicationDbContext context, ILogger<Details> logger) {
            _context = context;
            _logger = logger;
        }

        [FromRoute] public string OrgSlug { get; set; }

        public Org Org { get; set; }

        public async Task OnGetAsync() {
            Org = await _context.Orgs
                                .Include(o => o.Mods)
                                .ThenInclude(m => m.Latest)
                                .ThenInclude(v => v.VersionNumber)
                                .Include(o => o.Members)
                                .ThenInclude(m => m.User)
                                .ThenInclude(u => u.ShadowOrg)
                                .FirstOrDefaultAsync(o => o.Slug == OrgSlug);

            ViewData["BreadcrumbNode"] = new RazorPageBreadcrumbNode("/Users/Details", Org.DisplayName) {
                RouteValues = new {UserSlug = OrgSlug}
            };

        }

    }

}