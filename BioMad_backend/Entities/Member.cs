using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Member : IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public DateTime DateCreatedAt { get; set; }

        public DateTime DateBirthday { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.UtcNow;
                var age = today.Year - DateBirthday.Year;

                if (DateBirthday.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public int GenderId { get; set; }
        [JsonIgnore] public virtual Gender Gender { get; set; }

        [JsonIgnore] public virtual IEnumerable<MemberAnalysis> Analyzes { get; set; }
        public IEnumerable<int> AnalysisIds => Analyzes.Select(x => x.Id);
        [JsonIgnore] public virtual IEnumerable<MemberCategoryState> CategoryStates { get; set; }

        [JsonIgnore] public int UserId { get; set; }
        [JsonIgnore] public virtual User User { get; set; }

        public Member()
        {
            DateCreatedAt = DateTime.UtcNow;
        }
    }
}