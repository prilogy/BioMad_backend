using BioMad_backend.Entities;

namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface ILocalizable<out T>
    {
        T Localize(Culture culture);
    }
}