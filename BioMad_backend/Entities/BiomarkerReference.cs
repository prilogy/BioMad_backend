using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class BiomarkerReference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double ValueA { get; set; }
        public double ValueB { get; set; }
        [JsonIgnore]
        public int UnitId { get; set; }
        [JsonIgnore]
        public virtual Unit Unit { get; set; }
        public int BiomarkerId { get; set; }
        [JsonIgnore]
        public virtual Biomarker Biomarker { get; set; }
        
        public virtual BiomarkerReferenceConfig Config { get; set; }

        public BiomarkerReference InUnit(Unit unit)
        {
            var valA = UnitHelper.Convert(ValueA, Unit, unit);
            var valB = UnitHelper.Convert(ValueB, Unit, unit);

            if (valA == null || valB == null)
                return null;

            return new BiomarkerReference
            {
                Unit = unit,
                UnitId = unit.Id,
                Biomarker = Biomarker,
                BiomarkerId = BiomarkerId,
                Config = Config,
                ValueA = valA.Value,
                ValueB = valB.Value
            };
        }
    }
}