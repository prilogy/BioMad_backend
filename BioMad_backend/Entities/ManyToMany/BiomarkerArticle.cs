using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BioMad_backend.Entities.ManyToMany
{
    public class BiomarkerArticle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BiomarkerId { get; set; }
        [JsonIgnore]
        public virtual Biomarker Biomarker { get; set; }
        
        public int ArticleId { get; set; }
        [JsonIgnore]
        public virtual Article Article { get; set; }
        
        public int TypeId { get; set; }
        public virtual BiomarkerArticleType Type { get; set; }
    }
}