using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class UserEditModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}