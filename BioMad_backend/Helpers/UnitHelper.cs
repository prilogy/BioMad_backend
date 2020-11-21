using System.Linq;
using BioMad_backend.Entities;

namespace BioMad_backend.Helpers
{
    public static class UnitHelper
    {
        public static double? Convert(double value, Unit from, Unit to) // например from: кило, to: тонна
        {
            double? newValue = null;
            if (from.TransfersToIds.Contains(to.Id)) // если кило трансферится в тонну
                // берем значение и умножаем на коэф кило->тонна
                newValue = value * from.TransfersTo.FirstOrDefault(x => x.UnitB.Id == to.Id)?.Coefficient;
            else if (from.TransfersFromIds.Contains(to.Id)) // если кило транферится из тонны 
                // берем значение и делим на коэф тонна->кило
                newValue = value / from.TransfersFrom.FirstOrDefault(x => x.UnitA.Id == to.Id)?.Coefficient;
            
            return newValue;
        }
    }
}