using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class Gender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }

        public static readonly Gender Male = new Gender { Id = 1, Key = Keys.Male };
        public static readonly Gender Female = new Gender { Id = 2, Key = Keys.Female };
        public static readonly Gender Neutral = new Gender { Id = 3, Key = Keys.Neutral };


        public static class Keys
        {
            public const string Male = "male";
            public const string Female = "female";
            public const string Neutral = "neutral";
        }
    }


}