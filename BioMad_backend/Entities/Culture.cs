using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Culture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }
        public string Name { get; set; }

        [NotMapped, JsonIgnore] public CultureInfo Info { get; set; }

        public static Culture En = new Culture { Id = 1, Key = Keys.En, Name = "English", Info = new CultureInfo(Keys.En)};
        public static Culture Ru = new Culture { Id = 2, Key = Keys.Ru, Name = "Русский", Info = new CultureInfo(Keys.Ru)};

        public static List<Culture> All = new List<Culture> { En, Ru };

        public static Culture Fallback => En;

        public static class Keys
        {
            public const string Ru = "ru";
            public const string En = "en";
        }
    }
}