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
                return Ok(analysis); // LOCALIZE
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }
    }
}