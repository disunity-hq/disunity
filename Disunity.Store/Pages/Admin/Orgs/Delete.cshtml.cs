using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Orgs {

    [Breadcrumb("Delete", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public Org Org { get; set; }

        [BindProperty(SupportsGet = true, Name = "org")]
        public string OrgName { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            if (OrgName == null) {
                return NotFound();
            }

            Org = await _context.Orgs.FirstOrDefaultAsync(m => m.DisplayName == OrgName);

            if (Org == null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (OrgName == null) {
                return NotFound();
            }

            Org = await _context.Orgs.FirstOrDefaultAsync(m => m.DisplayName == OrgName);

            if (Org != null) {
                _context.Orgs.Remove(Org);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }

}