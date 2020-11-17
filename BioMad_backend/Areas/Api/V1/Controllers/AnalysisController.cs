using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.Interfaces;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AnalysisController : GetControllerBase<UserAnalysis>
    {
        public AnalysisController(ApplicationContext db, UserService userService) : base(db, userService)
        {
            
        }
        protected override IQueryable<UserAnalysis> Queryable => _db.UserAnalysis;

        public override Task<ActionResult<List<UserAnalysis>>> GetAll(int page, int pageSize, string orderByDate)
        {
            return Paging(_db.UserAnalysis.Where(x => x.UserId == _userService.UserId), page, pageSize, orderByDate);
        }
    }
}