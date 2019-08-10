using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Users {

    [Breadcrumb("Users", FromPage = typeof(Admin.IndexModel))]
    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<UserIdentity> UserIdentity { get; set; }

        public async Task OnGetAsync() {
            UserIdentity = await _context.Users.ToListAsync();
        }

    }

}