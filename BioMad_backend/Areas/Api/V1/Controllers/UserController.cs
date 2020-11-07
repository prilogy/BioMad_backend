using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ConfirmationService _confirmationService;

        public UserController(UserService userService, ConfirmationService confirmationService)
        {
            _userService = userService;
            _confirmationService = confirmationService;
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

        /// <summary>
        /// Sends email with code to confirm email
        /// </summary>
        /// <returns>Returns action result</returns>
        /// <response code="200">If email was sent</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpGet("email")]
        public async Task<IActionResult> EmailVerificationSend()
        {
            if (await _confirmationService.Send.Email(_userService.User))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Verifies user's email with given code
        /// </summary>
        /// <param name="code">Confirmation code to verify email</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPatch("email")]
        public async Task<IActionResult> EmailVerificationAttempt([Required] string code)
        {
            var confirmationCode =
                _confirmationService.Find(_userService.User, code, ConfirmationCode.Types.EmailConfirmation);

            if (confirmationCode == null)
                return BadRequest();

            if (await _confirmationService.Confirm(confirmationCode))
                return Ok();

            return BadRequest();
        }
    }
}