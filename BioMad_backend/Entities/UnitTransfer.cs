using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class UnitTransfer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int UnitAId { get; set; }
        [JsonIgnore]
        public virtual Unit UnitA { get; set; }

        public int UnitBId { get; set; }
        [JsonIgnore]
        public virtual Unit UnitB { get; set; }

        /// <summary>
        /// UnitA is UnitB * Coefficient
        /// </summary>
        public double Coefficient { get; set; }
    }
}