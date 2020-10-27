using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("logIn")]
        public async Task<IActionResult> LogInV1([Required]string email, [Required]string password)
        {
            return Ok(new { email, password });
        }

    }
}