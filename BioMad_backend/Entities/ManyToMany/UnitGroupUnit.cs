namespace BioMad_backend.Entities.ManyToMany
{
    public class UnitGroupUnit
    {
        public int UnitGroupId { get; set; }
        public virtual UnitGroup UnitGroup { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}