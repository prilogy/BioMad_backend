using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Category : ILocalizedEntity<CategoryTranslation>, ILocalizable<Category>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore] public virtual TranslationCollection<CategoryTranslation> Translations { get; set; }
        [NotMapped] public CategoryTranslation Content { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Biomarker> Biomarkers => CategoryBiomarkers.Select(x => x.Biomarker);

        #region [ Many to many ]

        [JsonIgnore]
        public virtual List<CategoryBiomarker> CategoryBiomarkers { get; set; }

        #endregion

        public Category Localize(Culture culture)
        {
            Content = Translations[culture];
            return this;
        }
    }

    public class CategoryTranslation : Translation<CategoryTranslation>, ITranslationEntity<Category>, IWithNameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore] public int BaseEntityId { get; set; }
        public Category BaseEntity { get; set; }
    }
}