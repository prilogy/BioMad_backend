using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class RefreshTokenAuthenticationModel
    {
        [Required]
        public string RefreshToken { get; set; }
        /// <summary>
        /// Id of current user
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Id of current member
        /// </summary>
        [Required]
        public int MemberId { get; set; }
    }
}