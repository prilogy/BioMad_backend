using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class MemberCategoryState : IWithDateCreated, IWithId, ILocalizable<MemberCategoryState>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public double State { get; set; }
        public double Difference { get; set; }

        public int CategoryId { get; set; }
        //[JsonIgnore]
        public virtual Category Category { get; set; }

        public int MemberId { get; set; }
        [JsonIgnore]
        public virtual Member Member { get; set; }
        
        public DateTime DateCreatedAt { get; set; }

        public MemberCategoryState Localize(Culture culture)
        {
            Category = Category?.Localize(culture);
            return this;
        }

        public MemberCategoryState()
        {
            DateCreatedAt = DateTime.UtcNow;
        }
    }
}