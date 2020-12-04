using System;
using System.Linq;
using System.Threading.Tasks;
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

            shared.Biomarkers = await _db.Biomarkers.Where(x => shared.BiomarkerIds.Contains(x.Id)).ToListAsync();
            shared.Biomarkers = shared.Biomarkers.Select(x => x.Process(cultureObj, shared.Member, _db)).ToList();
            
            return View(shared);
        }
    }
}