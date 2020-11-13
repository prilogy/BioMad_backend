using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerReference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Value { get; set; }
        
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        
        public int BiomarkerId { get; set; }
        public virtual Biomarker Biomarker { get; set; }
        
        public virtual BiomarkerReferenceConfig Config { get; set; }
    }
}