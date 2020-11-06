using System;
using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class SignUpModel
    {
        private string _email;
        [EmailAddress]
        [Required]
        public string Email
        {
            get => _email;
            set => _email = value.ToLower();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateTime DateBirthday { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string Color { get; set; }
    }

    
}