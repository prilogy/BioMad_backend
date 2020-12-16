using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Extensions;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class MemberBiomarker : IWithDateCreated, ILocalizable<MemberBiomarker>, IWithId, IUnitTransferable<MemberBiomarker>
    {
        public int Id { get; set; }
        
        public double Value { get; set; }
        
        public DateTime DateCreatedAt { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        
        public int BiomarkerId { get; set; }
        [JsonIgnore]
        public virtual Biomarker Biomarker { get; set; }

        [NotMapped] public BiomarkerTranslation BiomarkerContent => Biomarker?.Content;
        
        public int AnalysisId { get; set; }
        [JsonIgnore]
        public virtual MemberAnalysis Analysis { get; set; }

        public MemberBiomarker()
        {
            DateCreatedAt = DateTime.UtcNow;
        }

        public MemberBiomarker Localize(Culture culture)
        {
            Unit = Unit?.Localize(culture);
            Biomarker = Biomarker?.Localize(culture);
            return this;
        }

        public BiomarkerStateType GetState(BiomarkerReference reference)
        {
            if (reference == null)
                return BiomarkerStateType.NoInfo;

            if (reference.UnitId != UnitId)
                InUnit(reference.Unit);
            
            if (Value.IsBetween(reference.ValueA, reference.ValueB))
                return BiomarkerStateType.Normal;
            if (Value > reference.ValueB)
                return BiomarkerStateType.Higher;
            if (Value < reference.ValueA)
                return BiomarkerStateType.Lower;

            return BiomarkerStateType.NoInfo;
        }
        
        public bool? CalcIsNormal(Member member)
        {
            var reference = Biomarker.FindReference(member);
            if (reference == null)
                return null;
            
            var r = reference.UnitId == UnitId 
                ? reference 
                : reference.InUnit(Unit);

            if (r == null)
                return null;

            return Value.IsBetween(r.ValueA, r.ValueB);
        }

        public MemberBiomarker InUnit(Unit unit)
        {
            if (unit == null)
                return null;
            
            if (unit.Id == UnitId)
                return this;

            var newValue = UnitHelper.Convert(Value, Unit, unit);

            if (newValue == null)
                return null;

            return new MemberBiomarker
            {
                Analysis = Analysis,
                Biomarker = Biomarker,
                Unit = unit,
                UnitId = unit.Id,
                Id = Id,
                Value = newValue.Value,
                AnalysisId = AnalysisId,
                BiomarkerId = BiomarkerId,
                DateCreatedAt = DateCreatedAt
            };
        }
    }
}