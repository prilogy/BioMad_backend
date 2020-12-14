using BioMad_backend.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Admin.Controllers
{
  [Area("Admin")]
  [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = Role.Keys.Admin)]
  public class BiomarkerController : Controller
  {

  }
}