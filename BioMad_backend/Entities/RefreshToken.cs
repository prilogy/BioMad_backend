using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class RefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime DateExpiration { get; set; }

        public bool IsValid => DateExpiration > DateTime.UtcNow;
        
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        public RefreshToken()
        {
            DateExpiration = DateTime.UtcNow.AddMonths(6);
        }
    }
}