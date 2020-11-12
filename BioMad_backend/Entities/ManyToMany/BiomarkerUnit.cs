namespace BioMad_backend.Entities.ManyToMany
{
    public class BiomarkerUnit
    {
        public int BiomarkerId { get; set; }
        public virtual Biomarker Biomarker { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}