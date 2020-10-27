using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}