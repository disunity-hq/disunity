using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Policies;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;


namespace Disunity.Store.Pages.Users {

    [Breadcrumb]
    public class Details : PageModel {

        private readonly ILogger<Details> _logger;
        private readonly ApplicationDbContext _context;

        [FromRoute] public string UserSlug { get; set; }

        public UserIdentity UserIdentity { get; set; }

        public Details(ILogger<Details> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync() {
            UserIdentity = await _context.Users
                                         .Include(u => u.ShadowOrg)
                                         .ThenInclude(o => o.Mods)
                                         .ThenInclude(m => m.Latest)
                                         .ThenInclude(v => v.VersionNumber)
                                         .Include(u => u.ShadowOrg)
                                         .ThenInclude(o => o.Members)
                                         .ThenInclude(m => m.User)
                                         .ThenInclude(u => u.ShadowOrg)
                                         .FirstOrDefaultAsync(o => o.ShadowOrg.Slug == UserSlug);

            ViewData["BreadcrumbNode"] = new RazorPageBreadcrumbNode("/Users/Details", UserIdentity.ShadowOrg.Slug) {
                RouteValues = new {UserSlug}
            };

        }

    }

}