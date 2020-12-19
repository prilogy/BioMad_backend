using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class BiomarkerArticleType : ArticleType
    {
        
        public static readonly BiomarkerArticleType Increase = new BiomarkerArticleType
        {
            Id = 1,
            Key = "increase",
            Name = "Как повысить значение"
        };
        
        public static readonly BiomarkerArticleType Decrease = new BiomarkerArticleType
        {
            Id = 2,
            Key = "decrease",
            Name = "Как понизить значение"
        };

        public static readonly BiomarkerArticleType Decreased = new BiomarkerArticleType
        {
            Id = 3,
            Key = "decreased_desc",
            Name = "О пониженном значении"
        };

        public static readonly BiomarkerArticleType Increased = new BiomarkerArticleType
        {
            Id = 4,
            Key = "increased_desc",
            Name = "О повышенном значении"
        };
        
        public static readonly List<BiomarkerArticleType> All = new List<BiomarkerArticleType>
        {
            Increase,
            Decrease,
            Increased,
            Decreased
        };
    }
}