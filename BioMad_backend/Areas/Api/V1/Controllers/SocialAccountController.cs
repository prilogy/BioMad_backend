using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SocialAccountController : ControllerBase
    {
        private readonly IEnumerable<SocialAuthenticationService> _socialAuthenticationServices;
        private readonly ApplicationContext _applicationContext;
        private readonly UserService _userService;

        public SocialAccountController(IEnumerable<SocialAuthenticationService> socialAuthenticationServices,
            ApplicationContext applicationContext, UserService userService)
        {
            _socialAuthenticationServices = socialAuthenticationServices;
            _applicationContext = applicationContext;
            _userService = userService;
        }

        /// <summary>
        /// Gets social providers list
        /// </summary>
        /// <returns>Returns social providers list</returns>
        /// <response code="200">If everything went OK</response>
        [HttpGet("provider")]
        public async Task<ActionResult<IEnumerable<SocialAccountProvider>>> Providers() =>
            Ok(await _applicationContext.SocialAccountProviders.ToListAsync());

        /// <summary>
        /// Adds social account to current user
        /// </summary>
        /// <param name="model">Model contains token of social provider account</param>
        /// <param name="type">Social provider name</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="404">If provider isn't valid</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpPost("{type}")]
        public async Task<IActionResult> Add([Required] TokenModel model, string type)
        {
            var handler = GetSocialServiceHandler(type);
            if (handler == null)
                return NotFound();

            var account = await handler.CreateAccount(model.Token);
            if (account == null)
                return BadRequest();

            account.UserId = _userService.UserId;

            try
            {
                await _applicationContext.SocialAccounts.AddAsync(account);
                await _applicationContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Removes social account from current user
        /// </summary>
        /// <param name="type">Social provider name to delete</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="404">If provider isn't valid or social account doesn't exist</response>
        /// <response code="400">If anything went BAD</response> 
        [HttpDelete("{type}")]
        public async Task<IActionResult> Remove(string type)
        {
            var handler = GetSocialServiceHandler(type);
            if (handler == null)
                return NotFound();

            var account = _userService.User.SocialAccounts.FirstOrDefault(x => x.ProviderId == handler.Provider.Id);
            if (account == null)
                return NotFound();

            try
            {
                _applicationContext.Remove(account);
                await _applicationContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        private SocialAuthenticationService GetSocialServiceHandler(string type)
        {
            type = type.ToLower();
            return _socialAuthenticationServices.FirstOrDefault(x => x.Provider.Name.ToLower() == type);
        }
    }
}