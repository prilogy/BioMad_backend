using System;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class MemberBiomarker : IWithDateCreated, ILocalizable<MemberBiomarker>
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
    }
}