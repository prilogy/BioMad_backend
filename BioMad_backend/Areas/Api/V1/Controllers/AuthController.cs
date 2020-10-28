using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Entities;
using BioMad_backend.Models;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AuthController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Logs in user with given credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Authentication result</returns>
        /// <response code="200">If authentication is succeeded</response>
        /// <response code="404">If credentials are invalid</response>
        [HttpPost("logIn")]
        public async Task<ActionResult<AuthenticationResult>> LogIn(LogInWithCredentialsModel model)
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

        /// <summary>
        /// Authenticates user with model contains refresh token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Authentication result</returns>
        /// <response code="200">If everything went ok</response>
        /// <response code="400">If anything went wrong</response> 
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenAuthenticationModel model)
        {
            var result = await _authService.Authenticate(model);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            Console.WriteLine();
            return Ok(new
            {
                _userService.UserId,
                _userService.CurrentMemberId
            });
        }


        private async Task<IActionResult> SignUpInternal(Func<Task<User>> createUser)
        {
            try
            {
                var user = await createUser();
                if (user == null)
                    return Conflict();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}