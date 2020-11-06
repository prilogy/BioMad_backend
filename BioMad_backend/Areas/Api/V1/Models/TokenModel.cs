using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class TokenModel
    {
        [Required]
        public string Token { get; set; }
    }
}