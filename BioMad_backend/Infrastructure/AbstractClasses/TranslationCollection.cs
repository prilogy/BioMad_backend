using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using BioMad_backend.Entities;

namespace BioMad_backend.Infrastructure.AbstractClasses
{
    public class TranslationCollection<T> : Collection<T> where T : Translation<T>, new ()
    {
        
        public T this[Culture culture]
        {
            get => this.FirstOrDefault(x => x.CultureId == culture.Id) 
                   ?? this.FirstOrDefault(x => x.CultureId == Culture.Fallback.Id);
            set
            {
                var translation = this.FirstOrDefault(x => x.CultureId == culture.Id);
                if (translation != null)
                {
                    Remove(translation);
                }

                value.CultureId = culture.Id;
                Add(value);
            }
        }


        public bool HasCulture(Culture culture)
        {
            return this.Any(x => x.CultureId == culture.Id);
        }

    }
}