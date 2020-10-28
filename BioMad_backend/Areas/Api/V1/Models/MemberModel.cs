using System;
using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class MemberModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateTime DateBirthday { get; set; }
    }
}