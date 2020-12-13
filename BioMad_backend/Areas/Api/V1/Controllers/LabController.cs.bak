using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class LabController : GetControllerBase<Lab>
    {
        public LabController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<Lab> Queryable => _db.Labs;
        protected override Lab ProcessStrategy(Lab m) => m.Localize(_userService.Culture);
        
        /// <summary>
        /// Searches labs by query
        /// </summary>
        /// <param name="query">Query to search(by name)</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <returns>Result of search</returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<Lab>>> Search([FromBody, Required]string query, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
            => await Paging(_db.Labs.SearchWithQuery<Lab, LabTranslation>(query), page, pageSize, orderByDate);
    }
}