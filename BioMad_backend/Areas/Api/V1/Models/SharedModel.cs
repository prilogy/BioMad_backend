using System.Collections.Generic;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class SharedModel
    {
        public int MemberAnalysisId { get; set; }
        
        public List<int> BiomarkerIds { get; set; }
    }
}