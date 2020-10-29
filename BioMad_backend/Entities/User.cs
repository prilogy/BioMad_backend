using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class User : IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }

        public DateTime DateCreatedAt { get; set; }

        public virtual List<Member> Members { get; set; }
        
        [NotMapped]
        public int CurrentMemberId { get; set; }
        
        public virtual List<SocialAccount> SocialAccounts { get; set; }

        #region [ Role definition ]
        [JsonIgnore]
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
        
        #endregion
        
        [JsonIgnore]
        public virtual List<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            DateCreatedAt = DateTime.UtcNow;
            RoleId = Role.User.Id;
        }
    }
}