using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly ConfirmationService _confirmationService;
        private readonly PasswordService _passwordService;
        private readonly ApplicationContext _applicationContext;

        public PasswordResetController(UserService userService, ConfirmationService confirmationService, PasswordService passwordService, ApplicationContext applicationContext)
        {
            _confirmationService = confirmationService;
            _passwordService = passwordService;
            _applicationContext = applicationContext;
        }
        

        /// <summary>
        /// Sends confirmation email to user with given email
        /// </summary>
        /// <param name="email">Email of user that want to reset password</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response>
        [HttpGet]
        public async Task<IActionResult> ResetPassword([Required] string email)
        {
            email = email.ToLower();

            var user = await  _applicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return BadRequest();
            
            if (await _confirmationService.Send.PasswordReset(user))
                return Ok();
            
            return BadRequest();
        }

        /// <summary>
        /// Verifies code for password reset flow
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response>
        [HttpGet("verify")]
        public async Task<IActionResult> ResetPasswordVerify([Required] string code)
        {
            if (await _confirmationService.Find(code, ConfirmationCode.Types.PasswordReset) != null)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Resets password of user
        /// </summary>
        /// <param name="model">Model contains code and new password</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response>
        [HttpPatch]
        public async Task<IActionResult> ResetPasswordAttempt([Required] PasswordResetModel model)
        {
            var confirmationCode = await _confirmationService.Find(model.Code, ConfirmationCode.Types.PasswordReset);
            if (confirmationCode == null) return BadRequest();

            await using var transaction = await _applicationContext.Database.BeginTransactionAsync();
            try
            {
                var user = confirmationCode.User;
                user.Password = _passwordService.HashPassword(user, model.Password);

                if (await _confirmationService.Confirm(confirmationCode) != true)
                    throw new Exception();

                await _applicationContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }

        }
    }
}