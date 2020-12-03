using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class User : IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [EmailAddress] public string Email { get; set; }
        
        [NotMapped] public bool EmailIsVerified => ConfirmationCodes?.Any(x =>
            x.IsConfirmed && x.HelperValue == Email && x.Type == ConfirmationCode.Types.EmailConfirmation) ?? false;

        [JsonIgnore] public string Password { get; set; }

        public DateTime DateCreatedAt { get; set; }
        
        public int? CultureId { get; set; }
        [JsonIgnore]
        public virtual Culture _culture { get; set; }

        [NotMapped] public Culture Culture => Culture.All.FirstOrDefault(x => x.Id == CultureId) ?? Culture.Fallback;
        
        public virtual IEnumerable<Member> Members { get; set; }
        
        [JsonIgnore]
        public virtual IEnumerable<MemberAnalysis> Analyzes { get; set; } 

        [NotMapped] public int CurrentMemberId { get; set; }

        public virtual IEnumerable<SocialAccount> SocialAccounts { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<ConfirmationCode> ConfirmationCodes { get; set; }

        #region [ Role definition ]

        [JsonIgnore] public int RoleId { get; set; }
        [JsonIgnore] public virtual Role Role { get; set; }

        #endregion

        [JsonIgnore] public virtual IEnumerable<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            DateCreatedAt = DateTime.UtcNow;
            RoleId = Role.User.Id;
        }
    }
}