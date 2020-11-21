using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerReferenceConfigRange
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public double Lower { get; set; }
        public double Upper { get; set; }
    }
}