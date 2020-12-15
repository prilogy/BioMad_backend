using System.Net.Http;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        private readonly UserService _userService;

        public HomeController(UserService userService)
        {
            _userService = userService;
        }

        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}