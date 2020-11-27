using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Shared : IWithId, IWithDateCreated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime DateCreatedAt { get; set; }

        [NotMapped]
        public string Url { get; set; }

        public int MemberId { get; set; }
        [JsonIgnore]
        public virtual Member Member { get; set; }

        public List<int> BiomarkerIds { get; set; }

        public Shared()
        {
            DateCreatedAt = DateTime.UtcNow;
        }
    }
}