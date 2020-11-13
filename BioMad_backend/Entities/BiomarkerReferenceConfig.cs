using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerReferenceConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ReferenceId { get; set; }
        public virtual BiomarkerReference Reference { get; set; }
        
        public virtual IEnumerable<BiomarkerReferenceConfigDependency> Dependencies { get; set; }
        
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }
    }
}