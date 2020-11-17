using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Extensions
{
    public static class LocalizationExtension
    {
        public static T Localize<T, T2>(this T entity, Culture culture, bool localizeProperties = false)
            where T : ILocalizedEntity<T2>
            where T2 : Translation<T2>, new()
        {
            entity.Content = entity.Translations[culture];

            if (localizeProperties)
                LocalizeProperties(entity, culture);

            return (T) entity;
        }

        public static List<T> Localize<T, T2>(this List<T> collection, Culture culture, bool localizeProperties = false)
            where T : ILocalizedEntity<T2>
            where T2 : Translation<T2>, new()
            => collection.Select(x => x.Localize<T, T2>(culture, localizeProperties)).ToList();

        public static T LocalizeProperties<T>(T entity, Culture culture)
        {
            foreach (var property in entity.GetType().GetProperties())
            {
                var obj = property.GetValue(entity, null);
                var type = obj?.GetType();

                var types = type?.GetInterfaces()
                    .FirstOrDefault(x =>
                        x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ILocalizedEntity<>))
                    ?.GetGenericArguments(); 
                // TODO: добавить случай если список !!!!!!!!!!!!!!!

                var translationType = types?.FirstOrDefault(x => x.Name.ToLower().Contains("translation"));

                if (translationType == null)
                    continue;

                try
                {
                    var localize = typeof(LocalizationExtension)
                        .GetMethods()
                        .FirstOrDefault(x => x.Name == nameof(Localize) && !(x.ReturnType is IList));

                    localize?.MakeGenericMethod(type, translationType)
                        .Invoke(obj, new[] { obj, culture, true }); // Might be needed to set true here
                    
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return entity;
        }

        public static List<T> LocalizeProperties<T>(this List<T> list, Culture culture)
        {
            return list.Select(x => LocalizeProperties(x, culture)).ToList();
        }
    }
}