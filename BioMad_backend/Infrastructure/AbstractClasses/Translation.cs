using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Entities;
using Newtonsoft.Json;

namespace BioMad_backend.Infrastructure.AbstractClasses
{
    public abstract class Translation<T> where T: Translation<T>, new()
    {
        public int Id { get; set; }
        
        public int CultureId { get; set; }
        [JsonIgnore]
        public virtual Culture Culture { get; set; }

    }
}