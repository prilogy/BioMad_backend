namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface IUnitNormalizable<T>
    where T: class
    {
        T NormalizeUnits();
    }
}