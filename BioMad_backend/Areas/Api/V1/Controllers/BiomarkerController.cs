﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
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
        protected override Biomarker LocalizationStrategy(Biomarker m) => m.Localize(_userService.Culture);

        #region [ MemberBiomarker CRUD ]
        
        

        /// <summary>
        /// Adds new biomarker
        /// </summary>
        /// <param name="model">Model of biomarker</param>
        /// <returns>Id of newly created biomarker</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPost("member")]
        public async Task<ActionResult<int>> Add([Required] MemberBiomarkerModel model)
        {
            var biomarker = new MemberBiomarker
            {
                Value = model.Value,
                UnitId = model.UnitId,
                AnalysisId = model.AnalysisId,
                BiomarkerId = model.BiomarkerId
            };

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                await _db.MemberBiomarkers.AddAsync(biomarker);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(biomarker.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }

        /// <summary>
        /// Edits biomarker of given id
        /// </summary>
        /// <remarks>
        /// "AnalysisId" is ignored
        /// </remarks>
        /// <param name="model">New data</param>
        /// <param name="id">Id of biomarker</param>
        /// <returns>Biomarker with edited data</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPatch("member/{id}")]
        public async Task<ActionResult<MemberBiomarker>> Edit([Required] MemberBiomarkerModel model, int id)
        {
            var m = await FindMemberBiomarker(id);
            if (m == null)
                return BadRequest();
            
            if (model.Value != default)
                m.Value = model.Value;
            if (model.UnitId != default)
                m.UnitId = model.UnitId;
            if (model.BiomarkerId != default)
                m.BiomarkerId = model.BiomarkerId;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(m.Localize(_userService.Culture));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes biomarker of given id
        /// </summary>
        /// <param name="id">Id of biomarker</param>
        /// <returns>Action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpDelete("member/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await FindMemberBiomarker(id);
            if (m == null)
                return BadRequest();

            try
            {
                _db.Remove(m);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        
        private async Task<MemberBiomarker> FindMemberBiomarker(int id)
        {
            var m = await _db.MemberBiomarkers.FirstOrDefaultAsync(x => x.Id == id);
            if (m == null || m.Analysis?.Member?.UserId != _userService.UserId)
                return null;
            
            return m;
        }
    }
}