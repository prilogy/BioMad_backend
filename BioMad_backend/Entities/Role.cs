using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Serialization;

namespace BioMad_backend.Entities
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }


        public static Role User = new Role { Id = 1, Key = Keys.User };
        public static Role Admin = new Role { Id = 2, Key = Keys.Admin };

        public static RoleKeys Keys => new RoleKeys();
    }

    public class RoleKeys
    {
        public string Admin = "admin";
        public string User = "user";
    }
}