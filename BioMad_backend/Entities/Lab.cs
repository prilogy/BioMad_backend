using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Lab : ILocalizedEntity<LabTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string PhoneNumber { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }

            #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<LabTranslation> Translations { get; set; }
        [NotMapped] public LabTranslation Content { get; set; }

        #endregion
    }

    public class LabTranslation : Translation<LabTranslation>, ITranslationEntity<Lab>, IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore] public int BaseEntityId { get; set; }
        public Lab BaseEntity { get; set; }
    }
}