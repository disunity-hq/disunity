using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Util;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Pages.Mods {

    [Breadcrumb]
    public class Index : PageModel, IOrderableModel {

        private readonly ApplicationDbContext _context;

        public Index(ApplicationDbContext context) {
            _context = context;

        }

        public List<Mod> Mods { get; set; }

        [BindProperty(SupportsGet = true)] public string Title { get; set; }
        [BindProperty(SupportsGet = true)] public string Target { get; set; }

        [BindProperty(SupportsGet = true)] public string OrderBy { get; set; } = "Latest";

        [BindProperty(SupportsGet = true)] public Ordering? Order { get; set; }

        public IEnumerable<string> OrderByChoices => OrderOptions.Keys;

        public Dictionary<string, Expression<Func<Mod, IComparable>>> OrderOptions { get; set; } =
            new Dictionary<string, Expression<Func<Mod, IComparable>>>() {
                {"Latest", m => m.Latest.CreatedAt},
                {"Downloads", m => m.Versions.Sum(v => v.Downloads)},
                {"Name", m => m.Latest.DisplayName},
            };

        public async Task OnGetAsync() {
            if (string.IsNullOrWhiteSpace(Target)) {
                ViewData["BreadcrumbNode"] = new RazorPageBreadcrumbNode("/Mods", "Mods");
            } else {
                var targetDisplayName = await _context.Targets
                                                      .Where(t => t.Slug == Target)
                                                      .Include(t => t.Latest)
                                                      .Select(t => t.Latest.DisplayName)
                                                      .SingleAsync();

                var targetsNode = new RazorPageBreadcrumbNode("/Targets/Index", "Targets")
                    {OverwriteTitleOnExactMatch = true};

                var targetNode = new RazorPageBreadcrumbNode($"/Targets/Details", targetDisplayName) {
                    OverwriteTitleOnExactMatch = true,
                    Parent = targetsNode,
                    RouteValues = new {targetSlug = Target}
                };

                var modsNode = new RazorPageBreadcrumbNode("/Mods", "Mods") {
                    OverwriteTitleOnExactMatch = true,
                    Parent = targetNode
                };

                ViewData["BreadcrumbNode"] = modsNode;
            }

            Mods = await GetFilteredMods();
        }

        private async Task<List<Mod>> GetFilteredMods() {
            IQueryable<Mod> mods = _context.Mods
                                           .Include(m => m.Owner)
                                           .Include(m => m.Latest)
                                           .ThenInclude(v => v.VersionNumber);

            if (!string.IsNullOrWhiteSpace(Title)) {
                mods = mods.Where(m => EF.Functions.ILike(m.Latest.DisplayName, $"%{Title}%"));
            }

            if (!string.IsNullOrWhiteSpace(Target)) {
                mods = mods.Where(m => m.Versions.Any(v => v.TargetCompatibilities.Any(c => c.Target.Slug == Target)));
            }

            if (!string.IsNullOrWhiteSpace(OrderBy)) {

                var ordering = OrderOptions.GetValueOrDefault(OrderBy);

                if (ordering != null) {
                    if (Order != null && Order == Ordering.Desc) {
                        mods = mods.OrderByDescending(ordering);
                    } else {
                        mods = mods.OrderBy(ordering);
                    }

                }
            }


            return await mods.ToListAsync();
        }

    }

}