using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;

namespace BioMad_backend.Entities
{
    public class UnitGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? MainUnitId { get; set; }
        [NotMapped]
        public virtual Unit MainUnit { get; set; }
        
        public virtual IEnumerable<UnitGroupUnit> UnitGroups { get; set; }

        [NotMapped] public IEnumerable<Unit> Units => UnitGroups.Select(x => x.Unit);
        [NotMapped] public IEnumerable<int> UnitIds => UnitGroups.Select(x => x.UnitId);
    }
}