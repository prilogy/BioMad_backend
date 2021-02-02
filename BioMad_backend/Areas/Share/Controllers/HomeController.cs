using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Share.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.LocalizationResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BioMad_backend.Areas.Share.Controllers
{
    [Area("Share")]
    [Route("share")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ApplicationContext _db;
        
        public HomeController(IStringLocalizer<HomeController> localizer, ApplicationContext db)
        {
            _localizer = localizer;
            _db = db;
        }
        
        

        [HttpGet("{token}")]
        public async Task<IActionResult> Index(string token, [FromQuery]string culture)
        {
            ViewData["token"] = token;
            var cultureObj = Culture.All.FirstOrDefault(x => x.Key == culture);
            if (cultureObj == null)
                return NotFound();
            
            var shared = await _db.Shared.FirstOrDefaultAsync(x => x.Token == token);
            if (shared == null)
                return NotFound();

            ViewBag.Member = shared.Member;

            shared.Biomarkers = await _db.Biomarkers.Where(x => shared.BiomarkerIds.Contains(x.Id)).ToListAsync();
            shared.Biomarkers = shared.Biomarkers.Select(x => x.Process(cultureObj, shared.Member, _db)).ToList();

            var member = shared.Member;
            member.Gender = member.Gender.Localize(cultureObj);
            
            var history = new Dictionary<int, List<MemberBiomarker>>();
            foreach (var b in shared.Biomarkers)
            {
                var values = (await _db.MemberBiomarkers.Where(x => x.BiomarkerId == b.Id && x.Analysis.MemberId == shared.MemberId).OrderByDescending(x => x.Id).Take(7).ToListAsync())
                    .Select(x => x.InUnit(b.CurrentValue.Unit)).ToList();
                history.Add(b.Id, values);
            }
            
            return View(new SharedModel
            {
                Member = member,
                Shared = shared,
                History = history
            });
        }
    }
}