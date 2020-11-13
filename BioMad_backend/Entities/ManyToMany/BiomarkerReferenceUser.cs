using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities.ManyToMany
{
    public class BiomarkerReferenceUser
    {
        public int BiomarkerReferenceId { get; set; }
        public virtual BiomarkerReference BiomarkerReference { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}