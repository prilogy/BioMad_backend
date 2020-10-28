using BioMad_backend.Entities;

namespace BioMad_backend.Models
{
    public class AuthenticationResult
    {
        public User User { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}