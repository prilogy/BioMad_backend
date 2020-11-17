using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Helpers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class GetControllerBaseLocalized<T, T2> : ControllerBase
        where T : ILocalizedEntity<T2>
        where T2 : Translation<T2>, new()
    {
        protected readonly ApplicationContext _db;
        protected readonly UserService _userService;

        protected abstract IQueryable<T> Queryable { get; }

        protected virtual int PAGE_SIZE => 10;
        protected virtual bool LOCALIZE_PROPERTIES => false;

        public GetControllerBaseLocalized(ApplicationContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        /// <summary>
        /// Gets paged resources of type
        /// </summary>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of resources of type</returns>
        [HttpGet]
        public async Task<ActionResult<List<T>>> GetAll([FromQuery] int page, [FromQuery] int pageSize,
            [FromQuery] string orderByDate)
        {
            return await Paging(Queryable.AsQueryable(), page, pageSize, orderByDate);
        }
        
        /// <summary>
        /// Gets paged resources of type of given ids
        /// </summary>
        /// <param name="ids">Ids of resources to get</param>
        /// <param name="page">Number of page to get(starts from 1)</param>
        /// <param name="pageSize">Number of objects on one page</param>
        /// <param name="orderByDate">Order by date(asc|desc)</param>
        /// <response code="200">If everything went OK</response>
        /// <returns>List of resources of type of given ids</returns>
        [HttpPost]
        public async Task<ActionResult<List<T>>> GetByIds(int[] ids, [FromQuery] int page, [FromQuery] int pageSize,
            [FromQuery] string orderByDate = null)
        {
            var list = Queryable.Where(x => ids.Contains(x.Id));
            return await Paging(list, page, pageSize, orderByDate);
        }

        /// <summary>
        /// Gets resource of type of given id
        /// </summary>
        /// <param name="id">Number of page to get(starts from 1)</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="204">If no resource was found</response> 
        /// <returns>Resource of type</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);

            if(entity != null)
                return Ok(entity.Localize<T, T2>(_userService.Culture, LOCALIZE_PROPERTIES));
            
            return NoContent();
        }

        // [HttpPost("{id}/search")]
        // public virtual async Task<IActionResult> Search([FromBody] string query, [FromRoute] int id)
        // {
        //     return await Task.Run(() => { return NotFound(); });
        // }

        protected async Task<ActionResult<List<T>>> Paging(IQueryable<T> q, int page, int pageSize, string orderByDate=null)
        {
            if (orderByDate == "asc")
                q = q.OrderBy(x => x.Id);
            else if (orderByDate == "desc")
                q = q.OrderByDescending(x => x.Id);

            return Ok((page == 0
                ? await q.ToListAsync()
                : await q.Page(page, pageSize == 0 ? PAGE_SIZE : pageSize).ToListAsync()).Localize<T, T2>(_userService.Culture, LOCALIZE_PROPERTIES));
        }
    }
}