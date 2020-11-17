using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class MemberBiomarkerModel
    {
        public double Value { get; set; }
        public int AnalysisId { get; set; }
        [Required]
        public int BiomarkerId { get; set; }
        [Required]
        public int UnitId { get; set; }
    }
}