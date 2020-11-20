using System.Linq;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class CityController : GetControllerBase<City>
    {
        public CityController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<City> Queryable => _db.Cities;
        protected override City LocalizationStrategy(City m) => m.Localize(_userService.Culture);
    }
}