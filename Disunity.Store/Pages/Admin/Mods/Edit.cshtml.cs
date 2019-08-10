using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Mods {

    [Breadcrumb("Edit", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel {

        
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public Mod Mod { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Mod = await _context.Mods
                                .Include(m => m.Latest)
                                .Include(m => m.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (Mod == null) {
                return NotFound();
            }

            ViewData["LatestId"] = new SelectList(_context.ModVersions, "ID", "Description");
            ViewData["OwnerId"] = new SelectList(_context.Orgs, "ID", "ID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(Mod).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ModExists(Mod.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ModExists(int id) {
            return _context.Mods.Any(e => e.Id == id);
        }

    }

}