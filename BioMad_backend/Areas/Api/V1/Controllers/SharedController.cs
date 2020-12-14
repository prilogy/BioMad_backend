using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/v1/[controller]")]
  public class SharedController : GetControllerBase<Shared>
  {
    private readonly AppSettings _appSettings;

    public SharedController(ApplicationContext db, UserService userService, IOptions<AppSettings> options) : base(db,
      userService)
    {
      _appSettings = options.Value;
    }

    protected override IQueryable<Shared> Queryable =>
      _db.Shared.Where(x => x.MemberId == _userService.CurrentMemberId);

    protected override Shared ProcessStrategy(Shared m)
    {
      m.Url = _appSettings.SharedBaseUrl + m.Token;
      return m;
    }

    /// <summary>
    /// Adds new shared
    /// </summary>
    /// <param name="model">Model represents shared resources</param>
    /// <returns>Newly created shared resource</returns>
    /// <response code="200">If everything went OK</response>
    /// <response code="400">If anything went BAD</response> 
    [HttpPost("add")]
    public async Task<ActionResult<Shared>> Add([Required] SharedModel model)
    {
      var biomarkerIds = new List<int>();
      if (model.BiomarkerIds != null)
        biomarkerIds.AddRange(model.BiomarkerIds);

      var analysis = model.MemberAnalysisId != default
        ? _userService.CurrentMember.Analyzes.FirstOrDefault(x => x.Id == model.MemberAnalysisId)
        : null;

      if (analysis != null)
        biomarkerIds.AddRange(analysis.Biomarkers.Select(x => x.BiomarkerId).ToList());

      if (biomarkerIds.Count == 0)
        return BadRequest();


      biomarkerIds = biomarkerIds.Distinct().ToList();

      try
      {
        var shared = new Shared
        {
          Token = Hasher.RandomToken().Substring(0, 40)
            .Replace("/", "")
            .Replace(".","").Replace("?", ""),
          MemberId = _userService.CurrentMemberId,
          BiomarkerIds = biomarkerIds
        };

        await _db.AddAsync(shared);
        await _db.SaveChangesAsync();

        shared.Url = _appSettings.SharedBaseUrl + shared.Token +
                     $"?culture={_userService?.Culture?.Key ?? Culture.Fallback.Key}";

        return Ok(shared);
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    /// <summary>
    /// Deletes shared of given id
    /// </summary>
    /// <param name="id">Id of shared</param>
    /// <returns>Action result</returns>
    /// <response code="200">If everything went OK</response>
    /// <response code="400">If anything went BAD</response> 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var m = await _db.Shared.FirstOrDefaultAsync(x => x.Id == id && x.MemberId == _userService.CurrentMemberId);

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