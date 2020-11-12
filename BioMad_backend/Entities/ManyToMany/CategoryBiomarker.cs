namespace BioMad_backend.Entities.ManyToMany
{
    public class CategoryBiomarker
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        public int BiomarkerId { get; set; }
        public virtual Biomarker Biomarker { get; set; }
    }
}