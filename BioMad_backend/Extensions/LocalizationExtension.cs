using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Extensions
{
    public static class LocalizationExtension
    {
        public static T Localize<T, T2>(this ILocalizedEntity<T2> entity, Culture culture)
            where T : ILocalizedEntity<T2>
            where T2 : Translation<T2>, new()
        {
            entity.Content = entity.Translations[culture];
            return (T) entity;
        }

        public static List<T> Localize<T, T2>(this List<T> collection, Culture culture)
            where T: ILocalizedEntity<T2>
            where T2: Translation<T2>, new()
        => collection.Select(x => x.Localize<T, T2>(culture)).ToList();
    }
}