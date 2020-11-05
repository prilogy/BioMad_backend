using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        // TODO: add password reset action

        /// <summary>
        /// Gets user's account info
        /// </summary>
        /// <returns>Returns user data</returns>
        /// <response code="200">If user data successfully returned</response> 
        [HttpGet]
        public ActionResult<User> GetInfo() => Ok(_userService.User);

        /// <summary>
        /// Edits user's data
        /// </summary>
        /// <param name="model">New user data</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If user data was edited</response>
        /// <response code="400">If anything went BAD and user data wasn't edited</response> 
        [HttpPatch]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            if (await _userService.Edit(model))
                return Ok();

            return BadRequest();
        }
    }
}