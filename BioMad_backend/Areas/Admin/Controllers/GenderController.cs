using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class GenderController : LocalizedEntityController<Gender, GenderTranslation>
    {
        public GenderController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Gender> Queryable => _context.Genders;
    }
    
    public class GenderTranslationController : NoViewTranslationController<Gender, GenderTranslation>
    {
        public GenderTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Gender> Queryable => _context.Genders.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "Gender", new {id});
    }
}
