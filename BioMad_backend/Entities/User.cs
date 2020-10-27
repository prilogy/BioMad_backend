using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public virtual List<Member> Members { get; set; }
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        
    }
}