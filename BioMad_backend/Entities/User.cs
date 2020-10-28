using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Entities
{
    public class User : IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }

        public DateTime DateCreatedAt { get; set; }

        public virtual List<Member> Members { get; set; }
        
        [NotMapped]
        public Member CurrentMember { get; set; }


        #region [ Role definition ]

        [NotMapped]
        public int RoleId { get; set; }
        [NotMapped]
        public virtual Role Role { get; set; }
        

        #endregion
        public virtual List<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            DateCreatedAt = DateTime.UtcNow;
            RoleId = Role.User.Id;
        }
    }
}