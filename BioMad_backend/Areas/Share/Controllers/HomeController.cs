using System;
using BioMad_backend.Infrastructure.LocalizationResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BioMad_backend.Areas.Share.Controllers
{
    [Area("Share")]
    [Route("share")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("{token}")]
        public IActionResult Index(string token, [FromQuery]string culture)
        {
            ViewData["token"] = token;
            return View();
        }
    }
}