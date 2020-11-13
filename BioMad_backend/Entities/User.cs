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
        
        public bool EmailIsVerified => ConfirmationCodes.Any(x =>
            x.IsConfirmed && x.HelperValue == Email && x.Type == ConfirmationCode.Types.EmailConfirmation);

        [JsonIgnore] public string Password { get; set; }

        public DateTime DateCreatedAt { get; set; }
        
        public int? CultureId { get; set; }
        public virtual Culture Culture { get; set; }

        public virtual List<Member> Members { get; set; }

        [NotMapped] public int CurrentMemberId { get; set; }

        public virtual List<SocialAccount> SocialAccounts { get; set; }
        [JsonIgnore]
        public virtual List<ConfirmationCode> ConfirmationCodes { get; set; }

        #region [ Role definition ]

        [JsonIgnore] public int RoleId { get; set; }
        [JsonIgnore] public virtual Role Role { get; set; }

        #endregion

        [JsonIgnore] public virtual List<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            DateCreatedAt = DateTime.UtcNow;
            RoleId = Role.User.Id;
        }
    }
}