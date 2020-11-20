using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Biomarker : ILocalizedEntity<BiomarkerTranslation>, ILocalizable<Biomarker>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int TypeId { get; set; }
        public virtual BiomarkerType Type { get; set; }
        
        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<BiomarkerTranslation> Translations { get; set; }
        [NotMapped] public BiomarkerTranslation Content { get; set; }

        #endregion
        
        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Category> Categories => CategoryBiomarkers.Select(x => x.Category);
        
        [NotMapped]
        [JsonIgnore]
        private IEnumerable<Article> _articles => BiomarkerArticles.Select(x => x.Article);
        [NotMapped]
        public IEnumerable<Article> Articles { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Unit> _units => BiomarkerUnits.Select(x => x.Unit);
        [NotMapped]
        public IEnumerable<Unit> Units { get; set; }
        
        
        [JsonIgnore]
        public virtual List<BiomarkerReference> References { get; set; }

        #region [ Many to many ]

        [JsonIgnore]
        public virtual List<CategoryBiomarker> CategoryBiomarkers { get; set; }
        [JsonIgnore]
        public virtual List<BiomarkerArticle> BiomarkerArticles { get; set; }
        [JsonIgnore]
        public virtual List<BiomarkerUnit> BiomarkerUnits { get; set; }

        #endregion

        public Biomarker Localize(Culture culture)
        {
            Content = Translations[culture];
            Type = Type.Localize(culture);
            Articles = _articles.Localize(culture);
            Units = _units.Localize(culture);
            return this;
        }
    }

    public class BiomarkerTranslation : Translation<BiomarkerTranslation>, ITranslationEntity<Biomarker>,
        IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore] public int BaseEntityId { get; set; }
        public Biomarker BaseEntity { get; set; }
    }
}