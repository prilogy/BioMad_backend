using System.Collections.Generic;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelperController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;

        public HelperController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        
        /// <summary>
        /// Gets list of genders
        /// </summary>
        /// <returns>Returns list of genders</returns>
        /// <response code="200">If everything went OK</response>
        [HttpPost("gender")]
        public ActionResult<List<Gender>> Genders() => Ok(_applicationContext.Genders);
    }
}