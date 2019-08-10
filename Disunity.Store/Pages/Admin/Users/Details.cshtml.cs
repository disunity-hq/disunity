using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Users {

    [Breadcrumb("Details", FromPage = typeof(IndexModel))]
    public class DetailsModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) {
            _context = context;
        }

        public UserIdentity UserIdentity { get; set; }
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

    }

}