using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Users {

    [Breadcrumb("Create", FromPage = typeof(IndexModel))]
    public class CreateModel : PageModel {

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public UserIdentity UserIdentity { get; set; }

        public IActionResult OnGet() {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Users.Add(UserIdentity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }

}