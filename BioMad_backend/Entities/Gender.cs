using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Gender : ILocalizedEntity<GenderTranslation>, ILocalizable<Gender>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }
        
        [JsonIgnore]
        public virtual TranslationCollection<GenderTranslation> Translations { get; set; }
        
        [NotMapped]
        public GenderTranslation Content { get; set; }

        public Gender Localize(Culture culture)
        {
            Content = Translations?[culture];
            return this;
        }

        public Gender()
        {
            Translations = new TranslationCollection<GenderTranslation>();
        }
    }

    public class GenderTranslation : Translation<GenderTranslation>, ITranslationEntity<Gender>, IWithName
    {
        public string Name { get; set; }
        public int BaseEntityId { get; set; }
        [JsonIgnore]  public virtual Gender BaseEntity { get; set; }
    }
}