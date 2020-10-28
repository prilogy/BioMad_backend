using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Entities
{
    public class Member : IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime DateCreatedAt { get; set; }
        
        public DateTime DateBirthday { get; set; }

        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Member()
        {
            DateCreatedAt = DateTime.UtcNow;
        }
    }
}