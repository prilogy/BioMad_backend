using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BioMad_backend.Extensions
{
    public static class SearchExtension
    {
        public static IQueryable<T> SearchWithQuery<T, T2>(this IQueryable<T> set, string query)
            where T : class, ILocalizedEntity<T2>, ILocalizable<T>, new()
            where T2 : Translation<T2>, ITranslationEntity<T>, IWithName, new()
        {
            query ??= "";
            query = query.ToLower();
            return set.Where(x => x.Translations.Any(y => y.Name.ToLower().Contains(query)));
        }
    }
}