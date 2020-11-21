using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class CategoryController : GetControllerBase<Category>
    {
        private readonly MonitoringService _monitoringService;

        public CategoryController(ApplicationContext db, UserService userService, MonitoringService monitoringService) :
            base(db, userService)
        {
            _monitoringService = monitoringService;
        }

        protected override IQueryable<Category> Queryable => _db.Categories;

        protected override Category ProcessStrategy(Category m)
        {
            m = m.Localize(_userService.Culture);
            m.State = _monitoringService.CategoryStates.FirstOrDefault(x => x.CategoryId == m.Id);
            return m;
        }

        /// <summary>
        /// Gets state history of category with given id
        /// </summary>
        /// <param name="id">Category of which history to get</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of history states</returns>
        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<MemberCategoryState>>> GetHistoryById(int id, [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = "desc")
        {
            var list = _db.MemberCategoryStates
                .Where(x => x.CategoryId == id && x.MemberId == _userService.CurrentMemberId).AsQueryable();
            return await PagingExtension.Paging(list, page, (x) => x, pageSize,
                orderByDate);
        }

        /// <summary>
        /// Gets state history of categories
        /// </summary>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of history states</returns>
        [HttpGet("history")]
        public async Task<ActionResult<List<MemberCategoryState>>> GetHistory([FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string orderByDate = "desc")
        {
            var list = _db.MemberCategoryStates.Where(x => x.MemberId == _userService.CurrentMemberId).AsQueryable();
            return await PagingExtension.Paging(list, page, (x) => x.Localize(_userService.Culture), pageSize,
                orderByDate);
        }
    }
}