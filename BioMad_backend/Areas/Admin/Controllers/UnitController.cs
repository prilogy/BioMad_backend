using System;
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
    public class UnitController : LocalizedEntityController<Unit, UnitTranslation>
    {
        public UnitController(ApplicationContext context) : base(context)
        {
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditNew(int? id, string translationAction, int translationId, int cultureId)
        {
            return await base.Edit(id, translationAction, translationId, cultureId);
        }

        protected override IQueryable<Unit> Queryable => _context.Units.AsQueryable();
    }
    
    public class UnitTranslationController : NoViewTranslationController<Unit, UnitTranslation>
    {
        public UnitTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Unit> Queryable => _context.Units.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "Unit", new {id});
    }
}
