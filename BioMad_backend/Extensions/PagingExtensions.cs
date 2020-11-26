using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Extensions
{
    public static class PagingExtension
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
            => source.Skip((page - 1) * pageSize).Take(pageSize);

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
            => source.Skip((page - 1) * pageSize).Take(pageSize);
        
        public static async Task<ActionResult<List<T>>> Paging<T>(IQueryable<T> q, int page, Func<T,T> loc, int pageSize = default, string orderByDate=null)
        where T: IWithId
        {
            if(pageSize == default)
                pageSize = 10;

            q = orderByDate switch
            {
                "asc" => q.OrderBy(x => x.Id),
                "desc" => q.OrderByDescending(x => x.Id),
                _ => q
            };

            var l = page == default
                ? await q.ToListAsync()
                : await q.Page(page, pageSize).ToListAsync();

            return new OkObjectResult(l.Select(loc));
        }
    }
}