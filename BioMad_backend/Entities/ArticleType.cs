using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class ArticleType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Key { get; set; }
    }
}