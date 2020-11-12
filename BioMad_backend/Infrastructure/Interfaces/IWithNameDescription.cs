namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface IWithName
    {
        public string Name { get; set; }
    }

    public interface IWithDescription
    {
        public string Description { get; set; }
    }

    public interface IWithNameDescription : IWithName, IWithDescription
    {
    }
}