using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Mods {

    [Breadcrumb("Delete", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Mod = await _context.Mods.FindAsync(id);

            if (Mod != null) {
                _context.Mods.Remove(Mod);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }

}