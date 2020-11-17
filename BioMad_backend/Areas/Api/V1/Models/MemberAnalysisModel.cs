using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BioMad_backend.Entities;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class MemberAnalysisModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int LabId { get; set; }
        public DateTime Date { get; set; }
        
        [Required]
        public List<MemberBiomarkerModel> Biomarkers { get; set; }
    }
}