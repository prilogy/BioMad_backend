using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class UnitGroup : ILocalizedEntity<UnitGroupTranslation>, ILocalizable<UnitGroup>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? MainUnitId { get; set; }
        [NotMapped] public virtual Unit MainUnit { get; set; }

        public virtual List<UnitGroupUnit> UnitGroups { get; set; }

        [NotMapped] public IEnumerable<Unit> Units => UnitGroups?.Select(x => x.Unit);
        [NotMapped] public IEnumerable<int> UnitIds => UnitGroups?.Select(x => x.UnitId);

        [JsonIgnore] public virtual TranslationCollection<UnitGroupTranslation> Translations { get; set; }
        [NotMapped] public UnitGroupTranslation Content { get; set; }

        public UnitGroup Localize(Culture culture)
        {
            Content = Translations?[culture];
            return this;
        }
    }

    public class UnitGroupTranslation : Translation<UnitGroupTranslation>, ITranslationEntity<UnitGroup>,
        IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BaseEntityId { get; set; }
        [JsonIgnore] public UnitGroup BaseEntity { get; set; }
    }
}