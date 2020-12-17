using BioMad_backend.Infrastructure.AbstractClasses;

namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface ILocalizedEntity<T>: IWithId 
        where T: Translation<T>, new()
    {
        int Id { get; set; }
        TranslationCollection<T> Translations { get; set; }
        T Content { get; set; }
    }
}