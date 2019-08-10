using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.OrgMembers {

    [Breadcrumb("Edit", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel {

        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public OrgMember OrgMember { get; set; }

        public async Task<IActionResult> OnGetAsync(string orgId, string userId) {
            if (orgId == null || userId == null) {
                return NotFound();
            }

            OrgMember = await _context.OrgMembers
                                      .Include(o => o.Org)
                                      .Include(o => o.User)
                                      .FirstOrDefaultAsync(m => m.User.UserName == userId && m.Org.DisplayName == orgId);

            if (OrgMember == null) {
                return NotFound();
            }

            ViewData["OrgId"] = new SelectList(_context.Orgs, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(OrgMember).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!OrgMemberExists(OrgMember.OrgId, OrgMember.UserId)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrgMemberExists(int? orgId, string userId) {
            if (orgId == null || userId == null) {
                return false;
            }

            return _context.OrgMembers.Any(e => e.OrgId == orgId && e.UserId == userId);
        }

    }

}