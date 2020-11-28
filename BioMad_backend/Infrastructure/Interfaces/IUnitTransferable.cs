using BioMad_backend.Entities;

namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface IUnitTransferable<T>
    where T: class

    {
        T InUnit(Unit unit);
    }
}