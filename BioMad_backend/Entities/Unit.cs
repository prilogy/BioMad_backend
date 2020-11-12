using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Unit : ILocalizedEntity<UnitTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore] public virtual TranslationCollection<UnitTranslation> Translations { get; set; }
        [NotMapped] public UnitTranslation Content { get; set; }

        [NotMapped]
        public IEnumerable<Biomarker> Biomarkers => BiomarkerUnits.Select(x => x.Biomarker);

        [NotMapped] public IEnumerable<Unit> TransferableTo => Transfers.Select(x => x.UnitB);
        
        
        #region [ Many to many ]

        [JsonIgnore]
        public virtual List<BiomarkerUnit> BiomarkerUnits { get; set; }
        [JsonIgnore]
        public virtual List<UnitTransfer> Transfers { get; set; }

        #endregion
    }
    
    public class UnitTranslation : Translation<UnitTranslation>, ITranslationEntity<Unit>, IWithName
    {
        public string Name { get; set; }
        [JsonIgnore] public int BaseEntityId { get; set; }
        public Unit BaseEntity { get; set; }
    }
}