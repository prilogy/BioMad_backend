using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Route("api/v1/member/biomarker")]
    public class MemberBiomarkerController : GetControllerBase<MemberBiomarker>
    {
        private readonly MonitoringService _monitoringService;

        public MemberBiomarkerController(ApplicationContext db, UserService userService,
            MonitoringService monitoringService) : base(db, userService)
        {
            _monitoringService = monitoringService;
        }

        protected override IQueryable<MemberBiomarker> Queryable => _db.MemberBiomarkers.Where(x =>
            _userService.CurrentMember.Analyzes.Select(y => y.Id).Contains(x.AnalysisId));

        protected override MemberBiomarker ProcessStrategy(MemberBiomarker m) => m.Localize(_userService.Culture);
        
        /// <summary>
        /// Gets resource of type of given id
        /// </summary>
        /// <param name="id">Number of page to get(starts from 1)</param>
        /// <param name="unitId">Id of unit to represent the values</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If resource can't be translated to Unit of given unitId</response>
        /// <response code="404">If no resource was found</response> 
        /// <returns>Resource of type</returns>
        [HttpGet("{id}")]
        public override async Task<ActionResult<MemberBiomarker>> GetById(int id, int unitId = default)
        {
            var unit = await _db.Units.FirstOrDefaultAsync(x => x.Id == unitId);
            if (unit == null && unitId != default)
                return BadRequest();

            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();
            
            entity = ProcessStrategy(entity);
            return Ok(unit == null ? entity : entity.InUnit(unit));
        }

        #region [ MemberBiomarker CRUD ]

        /// <summary>
        /// Adds new member biomarker
        /// </summary>
        /// <param name="model">Model of biomarker</param>
        /// <returns>Id of newly created biomarker</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPost("add")]
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

                await _monitoringService.UpdateCategoryStates(new List<MemberBiomarker> { biomarker });

                return Ok(biomarker.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }

        /// <summary>
        /// Edits member's biomarker of given id
        /// </summary>
        /// <remarks>
        /// "AnalysisId" is ignored
        /// </remarks>
        /// <param name="model">New data</param>
        /// <param name="id">Id of biomarker</param>
        /// <returns>Biomarker with edited data</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPatch("{id}")]
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

                await _monitoringService.UpdateCategoryStates(new List<MemberBiomarker> { m });

                return Ok(m.Localize(_userService.Culture));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes member's biomarker of given id
        /// </summary>
        /// <param name="id">Id of biomarker</param>
        /// <returns>Action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await FindMemberBiomarker(id);
            if (m == null)
                return BadRequest();

            var changedBiomarkerIds = new List<int> { m.BiomarkerId };

            try
            {
                _db.Remove(m);
                await _db.SaveChangesAsync();

                await _monitoringService.UpdateCategoryStates(changedBiomarkerIds);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        /// <summary>
        /// Adds new member custom reference for biomarker
        /// </summary>
        /// <param name="model">Model of member reference</param>
        /// <returns>Action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPost("reference")]
        public async Task<IActionResult> AddReference([Required] MemberBiomarkerReferenceModel model)
        {
            var rf = await _db.MemberBiomarkerReferences
                .FirstOrDefaultAsync(x =>
                x.BiomarkerReference.BiomarkerId == model.BiomarkerId &&
                x.MemberId == _userService.CurrentMemberId);
            if (rf != null)
            {
                _db.Remove(rf.BiomarkerReference);
                _db.Remove(rf);
                await _db.SaveChangesAsync();
            }

            var reference = new BiomarkerReference
            {
                BiomarkerId = model.BiomarkerId,
                ValueA = model.ValueA,
                ValueB = model.ValueB,
                UnitId = model.UnitId
            };
            
            try
            {
                await _db.BiomarkerReferences.AddAsync(reference);
                await _db.SaveChangesAsync();

                await _db.MemberBiomarkerReferences.AddAsync(new MemberBiomarkerReference
                {
                    MemberId = _userService.CurrentMemberId,
                    BiomarkerReferenceId = reference.Id
                });

                await _db.SaveChangesAsync();
                await _monitoringService.UpdateCategoryStates(new[] { model.BiomarkerId });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private async Task<MemberBiomarker> FindMemberBiomarker(int id)
        {
            var m = await _db.MemberBiomarkers.FirstOrDefaultAsync(x => x.Id == id);
            if (m == null || m.Analysis?.Member?.UserId != _userService.UserId)
                return null;

            return m;
        }
    }
}