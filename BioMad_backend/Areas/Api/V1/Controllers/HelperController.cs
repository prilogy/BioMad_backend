using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelperController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserService _userService;
        private readonly MonitoringService _monitoringService;

        public HelperController(ApplicationContext applicationContext, UserService userService,
            MonitoringService monitoringService)
        {
            _applicationContext = applicationContext;
            _userService = userService;
            _monitoringService = monitoringService;
        }

        /// <summary>
        /// Gets list of genders
        /// </summary>
        /// <param name="culture">Culture of user</param>
        /// <returns>Returns list of genders</returns>
        /// <response code="200">If everything went OK</response>
        [HttpPost("gender")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Gender>>> Genders([FromHeader] string culture) =>
            Ok((await _applicationContext.Genders.ToListAsync()).Localize(_userService
                .Culture));

        /// <summary>
        /// Gets list of cultures
        /// </summary>
        /// <returns>Returns list of cultures</returns>
        /// <response code="200">If everything went OK</response>
        [HttpPost("culture")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Culture>>> Cultures() =>
            Ok(await _applicationContext.Cultures.ToListAsync());

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            var list = (await (_applicationContext.MemberCategoryStates
                    .FromSqlRaw(
                        "WITH summary AS (SELECT *, ROW_NUMBER() OVER(PARTITION BY p.\"CategoryId\" ORDER BY p.\"Id\" DESC) AS rk FROM \"MemberCategoryStates\" p WHERE \"MemberId\"={0}) SELECT s.* FROM summary s WHERE s.rk = 1",
                        1)
                ).ToListAsync());

            return Ok(list);
        }

        [HttpGet("test2")]
        [AllowAnonymous]
        public async Task<IActionResult> Test2()
        {
            // var lst = _applicationContext.MemberCategoryStates
            //     .Where(x => x.MemberId == 1)
            //     .GroupByAndSelect((m) => new
            //         {
            //             m.CategoryId
            //         },
            //         (m, x) => m.CategoryId == x.CategoryId,
            //         q => q.FirstOrDefault());

            var q = _applicationContext.MemberCategoryStates
                .Where(x => x.MemberId == 1);
            

            var s = q
                .Select(x =>
                    new
                    {
                        Key = new
                        {
                            x.CategoryId
                        },
                        Entity = x
                    });
            
            
            var query = s.Select(e => e.Key)
                .Distinct()
                .Select(key => s
                    .Where(e => e.Key.CategoryId == key.CategoryId)
                    .Select(x => x.Entity)
                    .OrderByDescending(x => x.Id)
                    .Take(1)
                );

            var lst = query.Select(x => x.FirstOrDefault()).AsEnumerable();

            return Ok(lst);
        } // TODO: test and complete
    }
}