using System.Collections.Generic;
using BioMad_backend.Entities;

namespace BioMad_backend.Areas.Api.V1.Models
{
    public class SearchResultModel
    {
        public List<Category> Categories { get; set; }
        public List<Unit> Units { get; set; }
        public List<Biomarker> Biomarkers { get; set; }
        public List<City> Cities { get; set; }
        public List<Lab> Labs { get; set; }
    }
}