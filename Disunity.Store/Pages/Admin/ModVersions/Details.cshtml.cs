using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.ModVersions {

    [Breadcrumb("Detail", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) {
            _context = context;
        }

        public ModVersion ModVersion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ModVersion = await _context.ModVersions.FirstOrDefaultAsync(m => m.Id == id);

            if (ModVersion == null) {
                return NotFound();
            }

            return Page();
        }

    }

}