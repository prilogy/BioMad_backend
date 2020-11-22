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

        private IEnumerable<MemberCategoryState> _categoryStates;

        public IEnumerable<MemberCategoryState> CategoryStates
        {
            get
            {
                if (_categoryStates == null)
                {
                    var s = _db.MemberCategoryStates
                        .Where(x => x.MemberId == _userService.CurrentMemberId)
                        .Select(x =>
                            new
                            {
                                Key = new
                                {
                                    x.CategoryId
                                },
                                MemberCategoryState = x
                            });
                    var query = s.Select(e => e.Key)
                        .Distinct()
                        .Select(key => s
                            .Where(e => e.Key.CategoryId == key.CategoryId)
                            .Select(x => x.MemberCategoryState)
                            .OrderByDescending(x => x.Id)
                            .Take(1)
                        );
                    _categoryStates = query.Select(x => x.FirstOrDefault()).AsEnumerable();
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

                    // TODO: add custom reference support
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
            var s = _db.MemberBiomarkers
                .Where(x => _userService.CurrentMember.AnalysisIds.Contains(x.AnalysisId) &&
                            biomarkerIds.Contains(x.BiomarkerId))
                .Include(x => x.Biomarker)
                .Include(x => x.Unit)
                .Select(x =>
                    new
                    {
                        Key = new
                        {
                            x.BiomarkerId
                        },
                        MemberBiomarker = x
                    });
            var query = s.Select(e => e.Key).Distinct()
                .Select(key => s
                    .Where(e => e.Key.BiomarkerId == key.BiomarkerId)
                    .Select(x => x.MemberBiomarker)
                    .OrderByDescending(x => x.Id)
                    .Take(1)
                );

            return await query.Select(x => x.FirstOrDefault()).ToListAsync();
        }
    }
}