using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class MemberAnalysis : IWithNameDescription, IWithDateCreated, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        
        public int MemberId { get; set; }
        [JsonIgnore]
        public virtual Member Member { get; set; }
        
        public int? LabId { get; set; }
        public virtual Lab Lab { get; set; }
        
        public virtual List<MemberBiomarker> Biomarkers { get; set; }
        
        public DateTime DateCreatedAt { get; set; }

        public MemberAnalysis()
        {
            DateCreatedAt = DateTime.UtcNow;
        }
    }
}