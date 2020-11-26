using System;
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
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class BiomarkerController : GetControllerBase<Biomarker>
    {
        public BiomarkerController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<Biomarker> Queryable => _db.Biomarkers;

        protected override Biomarker ProcessStrategy(Biomarker m)
        {
            m = m.Localize(_userService.Culture);
            m.Reference = m.FindReference(_userService.CurrentMember);
            m.CurrentValue = _db.MemberBiomarkers
                .Where(x => x.BiomarkerId == m.Id && _userService.CurrentMember.AnalysisIds.Contains(x.AnalysisId))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault()?.Localize(_userService.Culture);
            return m;
        }

        /// <summary>
        /// Gets history of member's biomarker values for given Id of biomarker
        /// </summary>
        /// <param name="id">Id of biomarker history to get</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of biomarker's history values for current member</returns>
        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<MemberBiomarker>>> GetHistory(int id, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = "desc")
        {
            var list = _db.MemberBiomarkers.Where(x => x.BiomarkerId == id).AsQueryable();
            return await PagingExtension.Paging(list, page, (x) => x.Localize(_userService.Culture), pageSize,
                orderByDate);
        }
        
        /// <summary>
        /// Gets types of biomarkers
        /// </summary>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of types</returns>
        [HttpGet("type")]
        public async Task<ActionResult<List<BiomarkerType>>> GetTypes([FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
        {
            var list = _db.BiomarkerTypes.AsQueryable();
            return await PagingExtension.Paging(list, page, (x) => x.Localize(_userService.Culture), pageSize,
                orderByDate);
        }
        
        /// <summary>
        /// Gets type of biomarker of given id
        /// </summary>
        /// <param name="id">Id of type to get</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="404">If no resource was found</response>
        /// <returns>Biomarker type of given id</returns>
        [HttpGet("type/{id}")]
        public async Task<ActionResult<List<BiomarkerType>>> GetTypeById(int id)
        {
            var t = await _db.BiomarkerTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (t == null)
                return NotFound();

            return Ok(t.Localize(_userService.Culture));
        }

        /// <summary>
        /// Searches biomarkers by query
        /// </summary>
        /// <param name="query">Query to search(by name)</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <returns>Result of search</returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<Biomarker>>> Search([FromBody, Required]string query, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
        => await Paging(_db.Biomarkers.SearchWithQuery<Biomarker, BiomarkerTranslation>(query), page, pageSize, orderByDate);

    }
}