using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Users {

    [Breadcrumb("Delete", FromPage = typeof(IndexModel))]
    public class DeleteModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
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
            if (Username == null) {
                return NotFound();
            }

            UserIdentity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);

            if (UserIdentity != null) {
                _context.Users.Remove(UserIdentity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }

}