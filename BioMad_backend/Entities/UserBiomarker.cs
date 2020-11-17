using System;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Entities
{
    public class UserBiomarker : IWithDateCreated
    {
        public int Id { get; set; }
        
        public double Value { get; set; }
        
        public DateTime DateCreatedAt { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        
        public int AnalysisId { get; set; }
        public virtual UserAnalysis Analysis { get; set; }
    }
}