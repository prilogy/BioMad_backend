using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using BioMad_backend.Services;

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

                if (type == null)
                    continue;

                var isList = type.GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                var entityType = isList
                    ? type.GetInterfaces()
                        .FirstOrDefault(x =>
                            x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                            && x.GetGenericArguments()
                                                .Any(y =>
                                                    y.GetInterfaces().Any(z =>
                                                        z.IsGenericType &&
                                                        z.GetGenericTypeDefinition() ==
                                                        typeof(ILocalizedEntity<>))))
                        ?.GetGenericArguments()
                        .FirstOrDefault(x =>
                            x.GetInterfaces().Any(y =>
                                y.IsGenericType &&
                                y.GetGenericTypeDefinition() ==
                                typeof(ILocalizedEntity<>)))
                    : type;

                var translationType = entityType?.GetInterfaces().FirstOrDefault(x =>
                        x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ILocalizedEntity<>))
                    ?.GetGenericArguments().FirstOrDefault(x => x.Name.ToLower().Contains("translation"));


                if (entityType == null || translationType == null)
                    continue;

                try
                {
                    var localize = typeof(LocalizationExtension)
                        .GetMethods()
                        .FirstOrDefault(x => x.Name == nameof(Localize) &&
                                             isList
                            ? x.ReturnType.IsGenericType && x.ReturnType.GetGenericTypeDefinition() == typeof(List<>)
                            : !(x.ReturnType.IsGenericType &&
                                x.ReturnType.GetGenericTypeDefinition() == typeof(List<>)));

                    localize?.MakeGenericMethod(entityType, translationType)
                        .Invoke(obj, new[] { obj, culture, true });
                }
                catch (Exception e)
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