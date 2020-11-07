using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class PasswordResetModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }
    }
}