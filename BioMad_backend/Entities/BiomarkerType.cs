using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class BiomarkerType : ILocalizedEntity<BiomarkerTypeTranslation>, ILocalizable<BiomarkerType>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore] public virtual TranslationCollection<BiomarkerTypeTranslation> Translations { get; set; }
        [NotMapped] public BiomarkerTypeTranslation Content { get; set; }
        [JsonIgnore] public virtual IEnumerable<Biomarker> Biomarkers { get; set; }

        [NotMapped] public IEnumerable<int> BiomarkerIds => Biomarkers.Select(x => x.Id);

        public BiomarkerType Localize(Culture culture)
        {
            Content = Translations[culture];
            return this;
        }
    }

    public class BiomarkerTypeTranslation : Translation<BiomarkerTypeTranslation>, ITranslationEntity<BiomarkerType>,
        IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int BaseEntityId { get; set; }
        public BiomarkerType BaseEntity { get; set; }
    }
}