using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Areas.Admin.Models;
using BioMad_backend.Data;
using BioMad_backend.Helpers;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace BioMad_backend.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class AuthController : AdminController
    {
        private readonly ApplicationContext _applicationContext;
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly AppSettings _appSettings;

        public AuthController(ApplicationContext applicationContext, AuthService authService, UserService userService,
            IOptions<AppSettings> options)
        {
            _applicationContext = applicationContext;
            _authService = authService;
            _userService = userService;
            _appSettings = options.Value;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (_userService.User != null)
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            var user = await _authService.AuthenticateCookies(model.Email, model.Password);
            if (user != null)
                return RedirectToAction("Index", "Home", new { Area = "Admin" });

            ModelState.AddModelError("", "Неправильный Email или пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Secret != _appSettings.AdminSecret)
            {
                ModelState.AddModelError("", "Неправильный секретный код");
                return View(model);
            }

            var user = await _userService.CreateAdmin(model.Email, model.Password);
            if (user != null)
                return LogIn();

            ModelState.AddModelError("", "Этот Email уже используется");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOutCookies();
            return RedirectToAction("LogIn", "Auth", new { Area = "Admin" });
        }
    }
}