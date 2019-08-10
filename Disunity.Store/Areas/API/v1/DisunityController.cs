using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;


namespace Disunity.Store.Areas.API.v1 {

    [ApiController]
    [Route("api/v{version:apiVersion}/disunity")]
    public class DisunityController {

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DisunityController(ApplicationDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisunityVersionDto>>> GetAllDisunityVersions() {
            var versions = await _dbContext.DisunityVersions
                                           .Include(v => v.VersionNumber)
                                           .Include(v => v.CompatibleUnityVersion)
                                           .ThenInclude(c => c.MinCompatibleVersion)
                                           .Include(v => v.VersionNumber)
                                           .Include(v => v.CompatibleUnityVersion)
                                           .ThenInclude(c => c.MaxCompatibleVersion)
                                           .Include(v => v.VersionNumber)
                                           .ProjectTo<DisunityVersionDto>(_mapper.ConfigurationProvider)
                                           .ToListAsync();

            return new JsonResult(versions, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Include});
        }

    }

}