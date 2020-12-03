using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Models;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialAuthenticationCore.Models;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEnumerable<SocialAuthenticationService> _socialAuthenticationServices;
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly ApplicationContext _applicationContext;

        public AuthController(UserService userService, AuthService authService,
            IEnumerable<SocialAuthenticationService> socialAuthenticationServices,
            ApplicationContext applicationContext)
        {
            _userService = userService;
            _authService = authService;
            _socialAuthenticationServices = socialAuthenticationServices;
            _applicationContext = applicationContext;
        }

        #region [ Normal auth flow ]

        /// <summary>
        /// Logs in user with given credentials
        /// </summary>
        /// <param name="model"></param>
        /// <param name="culture">Header represents current culture</param>
        /// <returns>Authentication result</returns>
        /// <response code="200">If authentication is succeeded</response>
        /// <response code="404">If credentials are invalid</response>
        [HttpPost("logIn")]
        public async Task<ActionResult<AuthenticationResult>> LogIn(LogInWithCredentialsModel model, [FromHeader] string culture)
        {
            var result = await _authService.Authenticate(model);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        /// <summary>
        /// Signs up new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns response code</returns>
        /// <response code="200">If sign up is successfully completed</response>
        /// <response code="409">If user with given email already exists</response>
        /// <response code="400">If something else went wrong</response> 
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            return await SignUpInternal(async () => await _userService.Create(model));
        }

        #endregion

        #region [ Refresh token auth flow ]

        /// <summary>
        /// Authenticates user with model contains refresh token;
        /// Revokes given refresh token and provides with new
        /// </summary>
        /// <param name="model"></param>
        /// <param name="culture">Header represents current culture</param>
        /// <returns>Authentication result</returns>
        /// <response code="200">If everything went ok</response>
        /// <response code="400">If anything went wrong</response> 
        [HttpPost("refreshToken")]
        public async Task<ActionResult<AuthenticationResult>> RefreshToken(RefreshTokenAuthenticationModel model, [FromHeader] string culture)
        {
            var result = await _authService.Authenticate(model);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        #endregion
        
        #region [ Social auth flow ]

        /// <summary>
        /// Logs in user by its social account
        /// </summary>
        /// <param name="model">Model contains token of social provider identity</param>
        /// <param name="type">Name of social provider</param>
        /// 
        /// <param name="culture">Header represents current culture</param>
        /// <returns>Returns action result</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If anything went BAD</response>
        /// <response code="404">If social provider is invalid</response> 
        [HttpPost("logIn/{type}")]
        public async Task<ActionResult<AuthenticationResult>> SocialLogIn([Required] TokenModel model, string type, [FromHeader] string culture)
        {
            var handler = GetSocialServiceHandler(type);
            if (handler == null)
                return NotFound();

            var user = await handler.FindUser(model.Token);
            if (user == null)
                return NotFound();

            var result = await _authService.Authenticate(user);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        /// <summary>
        /// Gets social provider identity to provide data for auth flow
        /// </summary>
        /// <param name="model">Model contains token of social account identity</param>
        /// <param name="type">Name of social provider</param>
        /// <returns>Returns social identity</returns>
        /// <response code="200">If everything went OK</response>
        /// <response code="400">If something else went wrong</response>
        /// <response code="404">If type is invalids</response> 
        [HttpPost("signUp/{type}/identity")]
        public async Task<ActionResult<SocialAuthenticationIdentity>> SocialSignUpInfo([Required]TokenModel model, string type)
        {
            var handler = GetSocialServiceHandler(type);
            
            if (handler == null)
                return NotFound();

            var identity = await handler.GetIdentity(model.Token);
            if (identity == null)
                return BadRequest();

            return Ok(identity);
        }

        /// <summary>
        /// Signs up new user with connected social account
        /// </summary>
        /// <param name="model">Data for User's sign up</param>
        /// <param name="type">Name of social provider</param>
        /// <returns>Returns response code</returns>
        /// <response code="200">If sign up is successfully completed</response>
        /// <response code="409">If user with given email already exists</response>
        /// <response code="400">If something else went wrong</response> 
        [HttpPost("signUp/{type}")]
        public async Task<IActionResult> SocialSignUp([Required] SignUpWithSocialAccountModel model, string type)
        {
            var handler = GetSocialServiceHandler(type);
            if (handler == null)
                return NotFound();

            var account = await handler.CreateAccount(model.Identity);
            if (account == null)
                return BadRequest();

            return await SignUpInternal(async () =>
            {
                try
                {
                    var user = await _userService.Create(model);
                    if (user == null)
                        return null;
                    
                    account.UserId = user.Id;
                    await _applicationContext.SocialAccounts.AddAsync(account);
                    await _applicationContext.SaveChangesAsync();
                    return user;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        #endregion

        #region [ Helper methods ]
        
        private async Task<IActionResult> SignUpInternal(Func<Task<User>> createUser)
        {
            try
            {
                var user = await createUser();
                if (user == null)
                    return Conflict();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        private SocialAuthenticationService GetSocialServiceHandler(string type)
        {
            type = type.ToLower();
            return _socialAuthenticationServices.FirstOrDefault(x => x.Provider.Name.ToLower() == type);
        }
        
        #endregion
    }
}