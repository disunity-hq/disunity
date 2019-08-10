using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.ModVersions {

    [Breadcrumb("Edit", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel {

        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
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

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(ModVersion).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ModVersionExists(ModVersion.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ModVersionExists(int id) {
            return _context.ModVersions.Any(e => e.Id == id);
        }

    }

}