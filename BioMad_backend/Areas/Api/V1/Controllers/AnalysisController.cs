using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Areas.Api.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.Interfaces;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AnalysisController : GetControllerBase<MemberAnalysis>
    {
        public AnalysisController(ApplicationContext db, UserService userService) : base(db, userService)
        {
            
        }

        protected override bool LOCALIZE_PROPERTIES => true;
        protected override IQueryable<MemberAnalysis> Queryable => _db.UserAnalysis.Where(x => x.MemberId == _userService.CurrentMemberId);
        
        /// <summary>
        /// Adds new analysis
        /// </summary>
        /// <param name="model">Model of analysis</param>
        /// <returns>Just added analysis</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPost("add")]
        public async Task<ActionResult<MemberAnalysis>> Add([Required] MemberAnalysisModel model)
        {
            var analysis = new MemberAnalysis
            {
                Name = model.Name,
                MemberId = _userService.CurrentMemberId,
                Date = model.Date,
                Description = model.Description,
                LabId = model.LabId
            };
            
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                await _db.MemberAnalyzes.AddAsync(analysis);
                await _db.SaveChangesAsync();

                var biomarkers = model.Biomarkers.Select(x => new MemberBiomarker
                {
                    AnalysisId = analysis.Id,
                    UnitId = x.UnitId,
                    Value = x.Value,
                    BiomarkerId = x.BiomarkerId
                });

                await _db.MemberBiomarkers.AddRangeAsync(biomarkers);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(Localize(analysis)); // LOCALIZE
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }

        /// <summary>
        /// Edits analysis of given id
        /// </summary>
        /// <param name="model">New data</param>
        /// <param name="id">Id of analysis</param>
        /// <returns>Analysis with edited data</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPatch("{id}")]
        public async Task<ActionResult<MemberAnalysis>> Edit([Required] MemberAnalysisModel model, int id)
        {
            var m = _userService.CurrentMember.Analyzes.FirstOrDefault(x => x.Id == id);
            if (m == null)
                return BadRequest();

            if (model.Name != null)
                m.Name = model.Name;
            if (model.Description != null)
                m.Description = model.Description;
            if (model.Date != default)
                m.Date = model.Date;
            if (model.LabId != default)
                m.LabId = model.LabId;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(Localize(m));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Deletes analysis of given id
        /// </summary>
        /// <param name="id">Id of analysis</param>
        /// <returns>Action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpDelete("{id}")]
        public async Task<ActionResult<MemberAnalysis>> Delete(int id)
        {
            var m = _userService.CurrentMember.Analyzes.FirstOrDefault(x => x.Id == id);
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
    }
}