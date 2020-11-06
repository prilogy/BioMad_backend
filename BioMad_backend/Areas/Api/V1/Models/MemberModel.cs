using System;
using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class MemberModel
    {
        public string Name { get; set; }
        public int GenderId { get; set; }
        public DateTime DateBirthday { get; set; }
        public string Color { get; set; }
    }
}