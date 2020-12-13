using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.LocalizationResources;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelperController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserService _userService;

        public HelperController(ApplicationContext applicationContext, UserService userService)
        {
            _applicationContext = applicationContext;
            _userService = userService;
        }

        /// <summary>
        /// Gets list of genders
        /// </summary>
        /// <param name="culture">Culture of user</param>
        /// <returns>Returns list of genders</returns>
        /// <response code="200">If everything went OK</response>
        [HttpPost("gender")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Gender>>> Genders([FromHeader] string culture) =>
            Ok((await _applicationContext.Genders.ToListAsync()).Localize(_userService
                .Culture));

        /// <summary>
        /// Gets list of cultures
        /// </summary>
        /// <returns>Returns list of cultures</returns>
        /// <response code="200">If everything went OK</response>
        [HttpPost("culture")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Culture>>> Cultures()
        => Ok(await _applicationContext.Cultures.ToListAsync());

        /// <summary>
        /// Searches resources by query
        /// </summary>
        /// <param name="query">Query to search(by name)</param>
        /// <returns>Result of search</returns>
        [HttpPost("search")]
        public async Task<ActionResult<SearchResultModel>> Search([FromBody, Required] string query)
            => Ok(new SearchResultModel
            {
                Biomarkers =
                    (await _applicationContext
                        .Biomarkers
                        .SearchWithQuery<Biomarker, BiomarkerTranslation>(query)
                        .ToListAsync()).Localize(_userService.Culture),
                Categories =
                    (await _applicationContext.Categories
                        .SearchWithQuery<Category, CategoryTranslation>(query)
                        .ToListAsync()).Localize(_userService.Culture),
                Cities = null,
                    // (await _applicationContext
                    //     .Cities
                    //     .SearchWithQuery<City, CityTranslation>(query)
                    //     .ToListAsync()).Localize(_userService.Culture),
                Labs = null,
                    // (await _applicationContext
                    //     .Labs
                    //     .SearchWithQuery<Lab, LabTranslation>(query)
                    //     .ToListAsync()).Localize(_userService.Culture),
                Units =
                    (await _applicationContext
                        .Units
                        .SearchWithQuery<Unit, UnitTranslation>(query)
                        .ToListAsync()).Localize(_userService.Culture)
            });
    }
}