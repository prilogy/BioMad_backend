using Newtonsoft.Json;

namespace BioMad_backend.Entities.ManyToMany
{
    public class BiomarkerArticle
    {
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