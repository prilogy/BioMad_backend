using System.Collections.Generic;
using System.Linq;

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
    }
}