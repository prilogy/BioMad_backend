using BioMad_backend.Infrastructure.AbstractClasses;

namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface ILocalizedEntity<T> where T: Translation<T>, new()
    {
        TranslationCollection<T> Translations { get; set; }
        T Content { get; set; }
    }
}