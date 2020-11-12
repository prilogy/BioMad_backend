using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerArticleType : ArticleType
    {
        
        public static readonly BiomarkerArticleType Increase = new BiomarkerArticleType
        {
            Id = 1,
            Key = "increase"
        };
        
        public static readonly BiomarkerArticleType Decrease = new BiomarkerArticleType
        {
            Id = 2,
            Key = "decrease"
        };
    }
}