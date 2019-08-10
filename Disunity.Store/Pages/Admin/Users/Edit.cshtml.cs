using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Users {

    [Breadcrumb("Edit", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel {

        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public UserIdentity UserIdentity { get; set; }

        [BindProperty(SupportsGet = true)] public string Username { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            if (Username == null) {
                return NotFound();
            }

            UserIdentity = await _context.Users.FirstOrDefaultAsync(m => m.UserName == Username);

            if (UserIdentity == null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(UserIdentity).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserIdentityExists(UserIdentity.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserIdentityExists(string id) {
            return _context.Users.Any(e => e.Id == id);
        }

    }

}