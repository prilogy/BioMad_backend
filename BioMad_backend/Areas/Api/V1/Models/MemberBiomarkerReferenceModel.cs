using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class MemberBiomarkerReferenceModel
    {
        [Required] public double ValueA { get; set; }
        [Required] public double ValueB { get; set; }
        [Required] public int UnitId { get; set; }
        [Required] public int BiomarkerId { get; set; }
    }
}