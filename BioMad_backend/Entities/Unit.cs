﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Unit : ILocalizedEntity<UnitTranslation>, ILocalizable<Unit>, IWithId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore] public virtual TranslationCollection<UnitTranslation> Translations { get; set; }
        [NotMapped] public UnitTranslation Content { get; set; }

        [NotMapped] [JsonIgnore] public IEnumerable<Biomarker> Biomarkers => BiomarkerUnits.Select(x => x.Biomarker);

        [NotMapped] public IEnumerable<int> TransfersToIds => TransfersTo.Select(x => x.UnitB.Id);
        [NotMapped] public IEnumerable<int> TransfersFromIds => TransfersFrom.Select(x => x.UnitA.Id);
        public virtual IEnumerable<UnitTransfer> TransfersTo { get; set; }
        public virtual IEnumerable<UnitTransfer> TransfersFrom { get; set; }

        #region [ Many to many ]

        [JsonIgnore] public virtual IEnumerable<BiomarkerUnit> BiomarkerUnits { get; set; }

        #endregion

        public Unit Localize(Culture culture)
        {
            Content = Translations[culture];
            return this;
        }
    }

    public class UnitTranslation : Translation<UnitTranslation>, ITranslationEntity<Unit>, IWithName
    {
        public string Name { get; set; }
        [JsonIgnore] public int BaseEntityId { get; set; }
        public Unit BaseEntity { get; set; }
    }
}