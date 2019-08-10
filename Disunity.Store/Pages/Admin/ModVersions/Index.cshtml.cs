using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.ModVersions {

    [Breadcrumb("Mod Versions", FromPage = typeof(Admin.IndexModel))]
    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<ModVersion> ModVersion { get; set; }

        public async Task OnGetAsync() {
            ModVersion = await _context.ModVersions.ToListAsync();
        }

    }

}