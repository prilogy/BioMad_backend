using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class Culture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }

        public static Culture En = new Culture { Id = 1, Key = Keys.En };
        public static Culture Ru = new Culture { Id = 2, Key = Keys.Ru };

        public static List<Culture> All = new List<Culture> { En, Ru };

        public static Culture Fallback => En;

        public static class Keys
        {
            public const string Ru = "ru";
            public const string En = "en";
        }
    }
}