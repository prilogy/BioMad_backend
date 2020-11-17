using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;

namespace BioMad_backend.Entities
{
    public class UserAnalysis : IWithNameDescription, IWithDateCreated, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public int? LabId { get; set; }
        public virtual Lab Lab { get; set; }
        
        public virtual List<UserBiomarker> Biomarkers { get; set; }
        
        public DateTime DateCreatedAt { get; set; }
    }
}