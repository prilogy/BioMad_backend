using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Logs in user with given credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Authentication result</returns>
        /// <response code="200">If authentication is succeeded</response>
        /// <response code="404">If credentials are invalid</response>
        [HttpPost("logIn")]
        public async Task<IActionResult> LogIn(LogInWithCredentialsModel model)
        {

            throw new NotImplementedException();
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