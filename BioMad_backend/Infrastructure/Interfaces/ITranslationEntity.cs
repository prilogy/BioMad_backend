namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface ITranslationEntity<T> where T: class
    {
        int BaseEntityId { get; set; }
        T BaseEntity { get; set; }
    }
}