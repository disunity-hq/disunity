using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Swagger;


namespace Disunity.Store.Areas.API.v1.Targets {

    [ApiController]
    [Route("api/v{version:apiVersion}/targets")]
    public class TargetController : ControllerBase {

        private readonly ILogger<TargetController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TargetController(ILogger<TargetController> logger, ApplicationDbContext context,
                                IMapper mapper) {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all targets currently registered with Disunity.io
        /// </summary>
        /// <example>Foobar</example>
        /// <returns>A JSON array of targets currently registered</returns>
        /// <response code="200" type="application/json">Returns a JSON array of targets currently registered</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TargetDto>>> GetAll() {
            var targets = await _context.Targets
                                        .Include(t => t.Latest)
                                        .ThenInclude(v => v.DisunityCompatibility)
                                        .ThenInclude(d => d.MinCompatibleVersion)
                                        .Include(t => t.Latest)
                                        .ThenInclude(v => v.DisunityCompatibility)
                                        .ThenInclude(d => d.MaxCompatibleVersion)
                                        .Include(t => t.Versions)
                                        .ProjectTo<TargetDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync();


            return new JsonResult(targets);
        }

        /// <summary>
        /// Lookup a target version from the executable hash
        /// </summary>
        /// <remarks>
        /// Search all versions of all targets for a version matching the specified hash. Will return an error if hash
        /// is not specified
        /// </remarks>
        /// <param name="hash">The SHA-256 hash of the executable you are searching for</param>
        /// <returns>Either details of the target version corresponding the given hash or an error if no version was found</returns>
        /// <response code="200">Returns details on the located target</response>
        /// <response code="404">Could not find a target with the given hash</response>
        /// <response code="400">Hash no provided</response>
        [HttpGet("find")]
        [Produces("application/json")]
        public async Task<ActionResult<TargetVersionDto>> FindTargetVersionByHash([Required, FromQuery] string hash) {
            var foundVersion = await _context.TargetVersions.Where(v => v.Hash == hash)
                                             .Include(v => v.DisunityCompatibility)
                                             .ThenInclude(d => d.MinCompatibleVersion)
                                             .Include(v => v.DisunityCompatibility)
                                             .ThenInclude(d => d.MaxCompatibleVersion)
                                             .ProjectTo<TargetVersionDto>(_mapper.ConfigurationProvider)
                                             .FirstOrDefaultAsync();


            if (foundVersion == null) {
                return new NotFoundResult();
            }

            return new JsonResult(foundVersion);
        }

    }

}