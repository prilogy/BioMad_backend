using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities.ManyToMany
{
    public class MemberBiomarkerReference
    {
        public int BiomarkerReferenceId { get; set; }
        public virtual BiomarkerReference BiomarkerReference { get; set; }

        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}