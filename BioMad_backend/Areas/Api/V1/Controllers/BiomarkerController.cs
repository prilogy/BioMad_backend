﻿using System;
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

        protected override Biomarker ProcessStrategy(Biomarker m) =>
            m.Process(_userService.Culture, _userService.CurrentMember, _db);

        /// <summary>
        /// Gets resource of type of given id
        /// </summary>
        /// <param name="id">Id of resource to get</param>
        /// <param name="unitId">Id of unit to represent the values</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If resource can't be translated to Unit of given unitId</response>
        /// <response code="404">If no resource was found</response> 
        /// <returns>Resource of type</returns>
        [HttpGet("{id}")]
        public override async Task<ActionResult<Biomarker>> GetById(int id, int unitId = default)
        {
            var unit = await _db.Units.FirstOrDefaultAsync(x => x.Id == unitId);
            if (unit == null && unitId != default)
                return BadRequest();

            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();
            
            entity = ProcessStrategy(entity);
            return Ok(unit == null ? entity : entity.InUnit(unit));
        }

        /// <summary>
        /// Gets history of member's biomarker values for given Id of biomarker
        /// </summary>
        /// <param name="id">Id of biomarker history to get</param>
        /// <param name="unitId">Id of unit to represent the values</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If resource can't be translated to Unit of given unitId</response>
        /// <returns>List of biomarker's history values for current member</returns>
        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<MemberBiomarker>>> GetHistory(int id, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = "desc", [FromQuery] int unitId = default)
        {
            var biomarker = await _db.Biomarkers.FirstOrDefaultAsync(x => x.Id == id);
            if (unitId == default && biomarker.MainUnitId != null)
                unitId = biomarker.MainUnitId.Value;
            
            var unit = await _db.Units.FirstOrDefaultAsync(x => x.Id == unitId);
            if (unit == null)
                return BadRequest();
            if (!biomarker.UnitGroup.UnitIds.Contains(unit.Id))
                return BadRequest();

            var list = _db.MemberBiomarkers.Where(x => x.BiomarkerId == id && x.Analysis.MemberId == _userService.CurrentMemberId).AsQueryable();
            return await PagingExtension.Paging(list, page, (x) => x.InUnit(unit).Localize(_userService.Culture),
                pageSize,
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
        public async Task<ActionResult<List<Biomarker>>> Search([FromBody, Required] string query, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
            => await Paging(_db.Biomarkers.SearchWithQuery<Biomarker, BiomarkerTranslation>(query), page, pageSize,
                orderByDate);
    }
}