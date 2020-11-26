using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class BiomarkerReferenceConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReferenceId { get; set; }
        [JsonIgnore]
        public virtual BiomarkerReference Reference { get; set; }

        public int? AgeRangeId { get; set; }
        public virtual BiomarkerReferenceConfigRange AgeRange { get; set; }
        
        public int GenderId { get; set; }
        [JsonIgnore]
        public virtual Gender Gender { get; set; }
    }
}