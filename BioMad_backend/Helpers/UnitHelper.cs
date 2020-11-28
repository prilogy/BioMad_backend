using System.Linq;
using BioMad_backend.Entities;

namespace BioMad_backend.Helpers
{
    public static class UnitHelper
    {
        public static double? Convert(double value, Unit from, Unit to)
        {
            double? newValue = null;
            if (from.TransfersToIds.Contains(to.Id))
                newValue = value * from.TransfersTo.FirstOrDefault(x => x.UnitB.Id == to.Id)?.Coefficient;
            else if (from.TransfersFromIds.Contains(to.Id))
                newValue = value / from.TransfersFrom.FirstOrDefault(x => x.UnitA.Id == to.Id)?.Coefficient;
            
            return newValue;
        }
    }
}