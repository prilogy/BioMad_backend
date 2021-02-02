using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
    public class Image : IWithId
    {
        public int Id { get; set; }
        public string Path { get; set; }
        [NotMapped] public string Url { get; set; }

        [JsonIgnore] public virtual List<ArticleImage> ArticleImages { get; set; }

        [JsonIgnore, NotMapped] public IEnumerable<Article> Articles => ArticleImages.Select(x => x.Article);

        public Image SetUrl(string baseUrl)
        {
            if (baseUrl[^1] == '/')
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);
            Url = baseUrl + Path;
            return this;
        }
    }
}