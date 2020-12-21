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
    public abstract class GetControllerBase<T> : ControllerBase
        where T : IWithId
    {
        protected readonly ApplicationContext _db;
        protected readonly UserService _userService;

        protected abstract IQueryable<T> Queryable { get; }

        protected virtual int PageSize => 10;
        protected abstract T ProcessStrategy(T m);

        public GetControllerBase(ApplicationContext db, UserService userService)
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
        public virtual async Task<ActionResult<List<T>>> GetAll([FromQuery] int page, [FromQuery] int pageSize,
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
        /// <param name="id">Id of resource to get</param>
        /// <param name="helperValue">Helper value not used here</param>
        /// <response code="200">If everything went OK</response>
        /// <response code="404">If no resource was found</response> 
        /// <returns>Resource of type</returns>
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(int id, int helperValue = default)
        {
            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);

            if(entity != null)
                return Ok(ProcessStrategy(entity));
            
            return NotFound();
        }

        protected async Task<ActionResult<List<T>>> Paging(IQueryable<T> q, int page, int pageSize,
            string orderByDate = null)
            => await PagingExtension.Paging(q, page, ProcessStrategy, pageSize, orderByDate ?? "desc");
    }
}