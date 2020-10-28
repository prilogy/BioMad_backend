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
    public class MemberController : ControllerBase
    {
        private readonly UserService _userService;

        public MemberController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Adds new member to user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If member was added</response>
        /// <response code="400">If anything went BAD and member wasn't added</response> 
        [HttpPost]
        public async Task<IActionResult> Add(MemberModel model)
        {
            if (await _userService.CreateMember(model))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Removes member out of user
        /// </summary>
        /// <param name="id">Id of member to remove from user</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If member was removed</response>
        /// <response code="400">If anything went BAD and member wasn't removed</response> 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (await _userService.RemoveMember(id))
                return Ok();

            return NotFound();
        }

        /// <summary>
        /// Edits data of member
        /// </summary>
        /// <param name="model">Model with new data</param>
        /// <param name="id">Id of member to edit</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If member was edited</response>
        /// <response code="400">If anything went BAD and member wasn't edited</response> 
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(MemberModel model, int id)
        {
            if (await _userService.EditMember(model, id))
                return Ok();

            return NotFound();
        }
    }
}