using System;
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

        [NotMapped] [JsonIgnore] public IEnumerable<Category> Categories => CategoryBiomarkers.Select(x => x.Category);
        [NotMapped] public IEnumerable<int> CategoryIds => CategoryBiomarkers.Select(x => x.CategoryId);

        [NotMapped] [JsonIgnore] public IEnumerable<Article> Articles { get; set; }
        [NotMapped] private IEnumerable<int> ArticleIds => BiomarkerArticles.Select(x => x.ArticleId);

        [NotMapped] [JsonIgnore] public IEnumerable<Unit> Units => BiomarkerUnits.Select(x => x.Unit);
        [NotMapped] public IEnumerable<int> UnitIds => BiomarkerUnits.Select(x => x.UnitId);

        [NotMapped] public BiomarkerReference Reference;
        [NotMapped] public MemberBiomarker CurrentValue;

        [JsonIgnore] public virtual List<BiomarkerReference> References { get; set; }

        #region [ Many to many ]

        [JsonIgnore] public virtual List<CategoryBiomarker> CategoryBiomarkers { get; set; }
        public virtual List<BiomarkerArticle> BiomarkerArticles { get; set; }
        [JsonIgnore] public virtual List<BiomarkerUnit> BiomarkerUnits { get; set; }

        #endregion

        public Biomarker Localize(Culture culture)
        {
            Content = Translations[culture];
            Type = Type.Localize(culture);
            return this;
        }

        public BiomarkerReference FindReference(Member member)
        {
            var referencesByGender = References.Where(x => x.MemberReference != null && x.MemberReference.MemberId == member.Id || (x.Config != null && x.Config.GenderId == member.GenderId)).ToList();

            var ownReference = referencesByGender.FirstOrDefault(x => x.IsOwnReference);
            if (ownReference != null)
                return ownReference;
            
            if (referencesByGender.Count == 0)
                referencesByGender = References;
            
            var referencesByAge = referencesByGender
                .OrderBy(x => x.Config.AgeRange).ToList();

            return  referencesByAge.FirstOrDefault(x => x.Config.AgeRange?.Lower > member.Age)
                ?? referencesByAge.LastOrDefault();
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