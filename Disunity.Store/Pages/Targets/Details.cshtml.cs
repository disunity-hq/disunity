using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Pages.Targets
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [FromRoute] public string TargetSlug { get; set; }
        [FromRoute] public string VersionNumber { get; set; }

        public Target Target { get; set; }

        public TargetVersion TargetVersion { get; set; }


        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Target = await _context.Targets.Include(t => t.Latest).SingleOrDefaultAsync(t => t.Slug == TargetSlug);
            if (Target == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(VersionNumber))
            {
                TargetVersion = Target.Latest;
            }
            else
            {
                TargetVersion = await _context.TargetVersions.SingleOrDefaultAsync(v => v.TargetId == Target.Id && v.VersionNumber == VersionNumber);
                if (TargetVersion == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }
    }
}