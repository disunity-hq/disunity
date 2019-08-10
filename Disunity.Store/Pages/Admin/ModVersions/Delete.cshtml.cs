using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.ModVersions {

    [Breadcrumb("Delete", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public ModVersion ModVersion { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ModVersion = await _context.ModVersions.FindAsync(id);

            if (ModVersion != null) {
                _context.ModVersions.Remove(ModVersion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }

}