using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class City : ILocalizedEntity<CityTranslation>, ILocalizable<City>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<CityTranslation> Translations { get; set; }
        [NotMapped] public CityTranslation Content { get; set; }
        [JsonIgnore] public virtual IEnumerable<Lab> Labs { get; set; }
        [NotMapped] public IEnumerable<int> LabIds => Labs.Select(x => x.Id);
        #endregion

        public City Localize(Culture culture)
        {
            Content = Translations[culture];
            return this;
        }
    }

    public class CityTranslation : Translation<CityTranslation>, ITranslationEntity<City>, IWithName
    {
        public string Name { get; set; }
        [JsonIgnore] public int BaseEntityId { get; set; }
        public City BaseEntity { get; set; }
    }
}