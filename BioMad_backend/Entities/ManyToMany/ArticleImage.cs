namespace BioMad_backend.Entities.ManyToMany
{
    public class ArticleImage
    {
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}