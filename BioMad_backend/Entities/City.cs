using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class City : ILocalizedEntity<CityTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<CityTranslation> Translations { get; set; }
        [NotMapped] public CityTranslation Content { get; set; }

        #endregion
    }

    public class CityTranslation : Translation<CityTranslation>, ITranslationEntity<City>, IWithName
    {
        public string Name { get; set; }
        [JsonIgnore] public int BaseEntityId { get; set; }
        public City BaseEntity { get; set; }
    }
}