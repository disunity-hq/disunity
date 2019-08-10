using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Route("api/v{version:apiVersion}/mods")]
    public class ModListController : ControllerBase {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ModListController> _logger;

        public ModListController(ApplicationDbContext context, IMapper mapper, ILogger<ModListController> logger) {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// Get a list of all mods registered with disunity.io
        /// </summary>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>An array of all found mods that are compatible with</returns>
        /// <response code="200">Return a JSON array of all mods registered with disunity.io</response>
        [HttpGet]
        [Produces("application/json")]
        public Task<ActionResult<IEnumerable<ModDto>>> GetAllMods([FromQuery] int page = 0,
                                                                        [FromQuery] int pageSize = 10) {
            return GetModsFromTarget(null, page, pageSize);
        }

        /// <summary>
        /// Get a list of all mods compatible with the given target id
        /// </summary>
        /// <remarks>
        /// Get a paginated list of all mods compatible with the given target id.
        /// If no pagination options are given or if the page is set to 0,
        /// then pagination will be skipped and all matching mods will be returned 
        /// </remarks>
        /// <param name="targetId">The target id to search for compatible mods</param>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>An array of all found mods that are compatible with</returns>
        /// <response code="200">Return a JSON array of all found mods compatible with the given target id</response>
        [HttpGet("{targetId}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ModDto>>> GetModsFromTarget([FromRoute] int? targetId=null,
                                                                               [FromQuery] int page = 0,
                                                                               [FromQuery] int pageSize = 10) {

            IQueryable<Mod> targetMods;

            if (targetId == null) {
                targetMods = _context.Mods;
            } else {
                targetMods = _context.Mods
                                     .Include(m => m.Versions)
                                     .ThenInclude(v => v.TargetCompatibilities)
                                     .Where(m => m.Versions.Any(
                                                v => v.TargetCompatibilities.Any(t => t.TargetId == targetId)));

                targetMods.ForEach(m => m.Versions = m.Versions
                                                      .Where(v => v.TargetCompatibilities.Any(
                                                                 t => t.TargetId == targetId))
                                                      .ToList());
            }

            var mappedTargetMods = await targetMods
                                         .OrderBy(v => v.Id)
                                         .Page(page, pageSize)
                                         .ProjectTo<ModDto>(_mapper.ConfigurationProvider)
                                         .ToListAsync();

            return new JsonResult(mappedTargetMods, new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Include});
        }

    }

}