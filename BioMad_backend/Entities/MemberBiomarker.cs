using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Extensions;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class MemberBiomarker : IWithDateCreated, ILocalizable<MemberBiomarker>, IWithId
    {
        public int Id { get; set; }
        
        public double Value { get; set; }
        
        public DateTime DateCreatedAt { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        
        public int BiomarkerId { get; set; }
        public virtual Biomarker Biomarker { get; set; }
        
        public int AnalysisId { get; set; }
        [JsonIgnore]
        public virtual MemberAnalysis Analysis { get; set; }

        public MemberBiomarker()
        {
            DateCreatedAt = DateTime.UtcNow;
        }

        public MemberBiomarker Localize(Culture culture)
        {
            Unit = Unit.Localize(culture);
            Biomarker = Biomarker.Localize(culture);
            return this;
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
    }
}