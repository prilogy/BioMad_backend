using BioMad_backend.Entities;

namespace BioMad_backend.Areas.Admin.Models
{
    public class BiomarkerReferenceModel
    {
        public int Id { get; set; }
        public int BiomarkerId { get; set; }
        
        public int GenderId { get; set; }
        public double AgeLower { get; set; }
        public double AgeUpper { get; set; }
        public int UnitId { get; set; }
        public double ValueA { get; set; }
        public double ValueB { get; set; }

        public static BiomarkerReferenceModel FromReference(BiomarkerReference r)
        {
            return new BiomarkerReferenceModel
            {
                Id = r.Id,
                BiomarkerId = r.BiomarkerId,
                GenderId = r.Config.GenderId,
                AgeLower = r.Config?.AgeRange?.Lower ?? 0,
                AgeUpper = r.Config?.AgeRange?.Upper ?? 0,
                UnitId = r.UnitId,
                ValueA = r.ValueA,
                ValueB = r.ValueB
            }
            ;
        }
    }
}