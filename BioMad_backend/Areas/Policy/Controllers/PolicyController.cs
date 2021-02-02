using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Policy.Controllers
{
    [Route("policy")]
    [Area("Policy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PolicyController : Controller
    {
        [HttpGet("ru")]
        public IActionResult Ru()
        {
            return View();
        }
        
        [HttpGet("en")]
        public IActionResult En()
        {
            return View();
        }
    }
}