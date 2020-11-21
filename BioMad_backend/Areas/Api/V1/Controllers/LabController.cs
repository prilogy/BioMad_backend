using System.Linq;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class LabController : GetControllerBase<Lab>
    {
        public LabController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<Lab> Queryable => _db.Labs;
        protected override Lab ProcessStrategy(Lab m) => m.Localize(_userService.Culture);
    }
}