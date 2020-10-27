using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class RefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime DateExpiration { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public RefreshToken()
        {
            DateExpiration = DateTime.UtcNow.AddDays(30);
        }
    }
}