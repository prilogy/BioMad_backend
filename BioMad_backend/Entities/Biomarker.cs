﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Data;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Biomarker : ILocalizedEntity<BiomarkerTranslation>, ILocalizable<Biomarker>,
        IUnitTransferable<Biomarker>, IUnitNormalizable<Biomarker>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TypeId { get; set; }
        public virtual BiomarkerType Type { get; set; }
        
        public int UnitGroupId { get; set; }
        [JsonIgnore] public virtual UnitGroup UnitGroup { get; set; }

        [JsonIgnore] public int? MainUnitId { get; set; }
        [JsonIgnore] public virtual Unit MainUnit { get; set; }

        [NotMapped] public int? DefaultUnitId => MainUnitId ?? UnitGroup?.MainUnitId;

        #region [ Localization ]

        [JsonIgnore] public virtual TranslationCollection<BiomarkerTranslation> Translations { get; set; }
        [NotMapped] public BiomarkerTranslation Content { get; set; }

        #endregion

        [NotMapped] [JsonIgnore] public IEnumerable<Category> Categories => CategoryBiomarkers?.Select(x => x.Category);
        [NotMapped] public IEnumerable<int> CategoryIds => CategoryBiomarkers?.Select(x => x.CategoryId);

        [NotMapped] [JsonIgnore] public IEnumerable<Article> Articles { get; set; }
        [NotMapped] private IEnumerable<int> ArticleIds => BiomarkerArticles?.Select(x => x.ArticleId);

        [NotMapped] [JsonIgnore] public IEnumerable<Unit> Units => UnitGroup?.Units;
        [NotMapped] public IEnumerable<int> UnitIds => Units?.Select(x => x.Id);

        [NotMapped] public BiomarkerReference Reference;
        [NotMapped] public MemberBiomarker CurrentValue;

        [JsonIgnore] public virtual List<BiomarkerReference> References { get; set; }

        #region [ Many to many ]

        [JsonIgnore] public virtual List<CategoryBiomarker> CategoryBiomarkers { get; set; }
        public virtual List<BiomarkerArticle> BiomarkerArticles { get; set; }

        [NotMapped] public BiomarkerStateType State => CurrentValue?.GetState(Reference) ?? BiomarkerStateType.NoInfo;

        #endregion

        public Biomarker Localize(Culture culture)
        {
            Content = Translations?[culture];
            Type = Type?.Localize(culture);
            return this;
        }

        public BiomarkerReference FindReference(Member member)
        {
            var referencesByGender = References.Where(x =>
                x.MemberReference != null && x.MemberReference.MemberId == member.Id ||
                (x.Config != null && x.Config.GenderId == member.GenderId)).ToList();

            var ownReference = referencesByGender.FirstOrDefault(x => x.IsOwnReference);
            if (ownReference != null)
                return ownReference;

            if (referencesByGender.Count == 0)
                referencesByGender = References;

            var referencesByAge = referencesByGender
                .OrderBy(x => x.Config.AgeRange).ToList();

            return referencesByAge.FirstOrDefault(x => x.Config.AgeRange?.Lower > member.Age)
                   ?? referencesByAge.LastOrDefault();
        }

        public Biomarker InUnit(Unit unit)
        {
            if (unit == null)
                return null;
            
            
            Reference = Reference?.InUnit(unit);
            
            if (CurrentValue == null || (CurrentValue != null && unit.Id == CurrentValue.UnitId))
                return this;

            var newCurrentValue = CurrentValue.InUnit(unit);
            if (newCurrentValue == null)
                return null;

            CurrentValue = newCurrentValue;
            return this;
        }

        public Biomarker NormalizeUnits()
        {
            if (CurrentValue == null || Reference == null || CurrentValue.UnitId == Reference.UnitId)
                return this;

            Reference = Reference.InUnit(CurrentValue.Unit);
            return this;
        }

        public Biomarker Process(Culture culture, Member member, ApplicationContext db)
        {
            Localize(culture);
            Reference = FindReference(member);
            if (MainUnit != null)
                Reference = Reference?.InUnit(MainUnit);
            CurrentValue = db.MemberBiomarkers
                .Where(x => x.BiomarkerId == Id && member.AnalysisIds.Contains(x.AnalysisId))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault()?.Localize(culture);

            NormalizeUnits();
            return this;
        }
    }
    
    public class BiomarkerTranslation : Translation<BiomarkerTranslation>, ITranslationEntity<Biomarker>,
        IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int BaseEntityId { get; set; }
        [JsonIgnore] public Biomarker BaseEntity { get; set; }
    }
    
    
    public enum BiomarkerStateType
    {
        Higher,
        Lower,
        Normal,
        NoInfo
    }
}