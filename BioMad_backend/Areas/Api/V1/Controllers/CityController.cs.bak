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
    public class CityController : GetControllerBase<City>
    {
        public CityController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<City> Queryable => _db.Cities;
        protected override City ProcessStrategy(City m) => m.Localize(_userService.Culture);
        
        /// <summary>
        /// Searches cities by query
        /// </summary>
        /// <param name="query">Query to search(by name)</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <returns>Result of search</returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<City>>> Search([FromBody, Required]string query, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
            => await Paging(_db.Cities.SearchWithQuery<City, CityTranslation>(query), page, pageSize, orderByDate);

    }
}