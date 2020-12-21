using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Article : ILocalizedEntity<ArticleTranslation>, ILocalizable<Article>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [JsonIgnore] public virtual List<ArticleImage> ArticleImages { get; set; }
        [JsonIgnore] public virtual List<BiomarkerArticle> BiomarkerArticles { get; set; }

        [JsonIgnore, NotMapped] public IEnumerable<Image> Images => ArticleImages?.Select(x => x.Image); 

        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<ArticleTranslation> Translations { get; set; }
        [NotMapped] public ArticleTranslation Content { get; set; }

        #endregion
        
        public Article Localize(Culture culture)
        {
            Content = Translations?[culture];
            return this;
        }
    }

    public class ArticleTranslation : Translation<ArticleTranslation>, ITranslationEntity<Article>, IWithName
    {
        public string Name { get; set; }
        public string Text { get; set; }
        
        public int BaseEntityId { get; set; }
        [JsonIgnore] public Article BaseEntity { get; set; }
    }
}