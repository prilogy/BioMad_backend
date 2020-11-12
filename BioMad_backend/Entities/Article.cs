using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Article : ILocalizedEntity<ArticleTranslation>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<ArticleTranslation> Translations { get; set; }
        [NotMapped] public ArticleTranslation Content { get; set; }

        #endregion
    }

    public class ArticleTranslation : Translation<ArticleTranslation>, ITranslationEntity<Article>, IWithName
    {
        public string Name { get; set; }
        public string Text { get; set; }
        
        [JsonIgnore] public int BaseEntityId { get; set; }
        public Article BaseEntity { get; set; }
    }
}