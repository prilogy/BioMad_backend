using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerReferenceConfigDependency
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public BiomarkerReferenceConfigDependencyType Type { get; set; }
        
        public int RangeId { get; set; }
        public virtual BiomarkerReferenceConfigDependencyRange Range { get; set; }
        
        public int ConfigId { get; set; }
        public virtual BiomarkerReferenceConfig Config { get; set; }
        
    }

    public enum BiomarkerReferenceConfigDependencyType
    {
        Age
    }
}