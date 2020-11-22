using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Extensions
{
    public static class LinqExtension
    {
        public static IEnumerable<T> GroupByAndSelect<T, TK>(this IQueryable<T> s, 
            Func<T, TK> key,
            Func<T, TK, bool> where, 
            Func<IQueryable<T>, T> select)
            where T : IWithId
        {
            var src = s
                .Select(x =>
                    new 
                    {
                        Key = key(x),
                        Entity = x
                    });

            var query = src.Select(e => e.Key)
                .Distinct()
                .Select(k => src
                    .Where(e => where(e.Entity, k))
                    .Select(x => x.Entity)
                    .OrderByDescending(x => x.Id)
                    .Take(1)
                );

            return query.Select(x => x.FirstOrDefault()).AsEnumerable();
        }
        
    }
}