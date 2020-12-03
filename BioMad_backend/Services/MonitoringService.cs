using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Services
{
  public class MonitoringService
  {
    private readonly ApplicationContext _db;
    private readonly UserService _userService;

    public MonitoringService(ApplicationContext db, UserService userService)
    {
      _db = db;
      _userService = userService;
    }

    private List<MemberCategoryState> _categoryStates;

    public List<MemberCategoryState> CategoryStates
    {
      get
      {
        if (_categoryStates == null)
        {
          var lst = _db.MemberCategoryStates.Where(x => x.MemberId == _userService.CurrentMemberId)
            .GroupBy(x => x.CategoryId)
            .Select(x =>
              x.Max(y => y.Id));

          _categoryStates = _db.MemberCategoryStates.Where(x => lst.Contains(x.Id)).ToList();
        }

        return _categoryStates;
      }
    }

    public async Task<bool> UpdateCategoryStates(IEnumerable<MemberBiomarker> biomarkers)
      => await UpdateCategoryStates(biomarkers.Select(x => x.BiomarkerId));


    public async Task<bool> UpdateCategoryStates(IEnumerable<int> biomarkerIds)
    {
      var categoriesToUpdate = await _db.Categories
        .Where(x => x.CategoryBiomarkers.Any(y => biomarkerIds.Contains(y.BiomarkerId)))
        .ToListAsync();

      var categoryStates = new List<MemberCategoryState>();

      foreach (var category in categoriesToUpdate)
      {
        try
        {
          var categoryBiomarkerIds = category.BiomarkerIds;
          var latestMemberBiomarkers = await GetLatestMemberBiomarkers(categoryBiomarkerIds);
          var normalCount = latestMemberBiomarkers.Aggregate(0,
            (acc, x) => acc + (x.CalcIsNormal(_userService.CurrentMember) == true ? 1 : 0));
          var newState = latestMemberBiomarkers.Count == 0
            ? 0
            : (double) normalCount
              / (double) latestMemberBiomarkers.Count;

          categoryStates.Add(new MemberCategoryState
          {
            CategoryId = category.Id,
            State = newState,
            Difference =
              newState - (CategoryStates.FirstOrDefault(x => x.CategoryId == category.Id)?.State ?? 0),
            MemberId = _userService.CurrentMemberId,
          });
        }
        catch (Exception)
        {
          continue;
        }
      }

      try
      {
        await _db.AddRangeAsync(categoryStates);
        await _db.SaveChangesAsync();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private async Task<List<MemberBiomarker>> GetLatestMemberBiomarkers(IEnumerable<int> biomarkerIds)
    {
      var ids = _db.MemberBiomarkers
        .Where(x => _userService.CurrentMember.AnalysisIds.Contains(x.AnalysisId) &&
                    biomarkerIds.Contains(x.BiomarkerId))
        .GroupBy(x => x.BiomarkerId)
        .Select(x => x.Max(y => y.Id));

      return await _db.MemberBiomarkers.Where(x => ids.Contains(x.Id))
        .Include(x => x.Biomarker)
        .Include(x => x.Unit)
        .Include(x => x.Analysis)
        .ToListAsync();
    }
  }
}