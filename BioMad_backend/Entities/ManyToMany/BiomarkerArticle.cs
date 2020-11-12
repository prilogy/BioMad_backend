namespace BioMad_backend.Entities.ManyToMany
{
    public class BiomarkerArticle
    {
        public int BiomarkerId { get; set; }
        public virtual Biomarker Biomarker { get; set; }
        
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        
        public int TypeId { get; set; }
        public virtual BiomarkerArticleType Type { get; set; }
    }
}