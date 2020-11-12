using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class BiomarkerType : ILocalizedEntity<BiomarkerTypeTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual TranslationCollection<BiomarkerTypeTranslation> Translations { get; set; }
        [NotMapped]
        public BiomarkerTypeTranslation Content { get; set; }
        
        public virtual List<Biomarker> Biomarkers { get; set; }
    }

    public class BiomarkerTypeTranslation : Translation<BiomarkerTypeTranslation>, ITranslationEntity<BiomarkerType>,
        IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public int BaseEntityId { get; set; }
        public BiomarkerType BaseEntity { get; set; }
    }
}