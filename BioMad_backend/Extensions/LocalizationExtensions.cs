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
        public static List<T> Localize<T>(this IEnumerable<T> collection, Culture culture)
            where T : ILocalizable<T>
            => collection.Select(x => x.Localize(culture)).ToList();
    }
}