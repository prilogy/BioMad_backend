using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Gender : ILocalizedEntity<GenderTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }
        
        [JsonIgnore]
        public virtual TranslationCollection<GenderTranslation> Translations { get; set; }
        
        [NotMapped]
        public GenderTranslation Content { get; set; }

        public static readonly Gender Male = new Gender { Id = 1, Key = Keys.Male };
        public static readonly Gender Female = new Gender { Id = 2, Key = Keys.Female };
        public static readonly Gender Neutral = new Gender { Id = 3, Key = Keys.Neutral };

        public Gender()
        {
            Translations = new TranslationCollection<GenderTranslation>();
        }

        public static class Keys
        {
            public const string Male = "male";
            public const string Female = "female";
            public const string Neutral = "neutral";
        }
    }

    public class GenderTranslation : Translation<GenderTranslation>, ITranslationEntity<Gender>
    {
        public string Name { get; set; }
        public int BaseEntityId { get; set; }
        public virtual Gender BaseEntity { get; set; }
    }
}