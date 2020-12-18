using System;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.Interfaces;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class UnitGroupController : LocalizedEntityController<UnitGroup, UnitGroupTranslation>
    {
        public UnitGroupController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<UnitGroup> Queryable => _context.UnitGroups.AsQueryable();
        
        public async Task<IActionResult> RemoveUnit(int entityId, int containerId, bool returnToEntity = false)
        {
            if(await NavigationPropertyHelpers.RemoveAsync<UnitGroup, UnitGroupUnit>(_context, entityId, containerId,
                x => x.UnitGroups.FirstOrDefault(y => y.UnitId == entityId)))
                return returnToEntity ? RedirectToAction("Edit", nameof(Unit), new { Area = "Admin", id = entityId}) : RedirectToEditById(containerId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddUnit(int entityId, int containerId, bool returnToEntity = false)
        {
            if (await NavigationPropertyHelpers.AddAsync<UnitGroup>(_context, entityId, containerId,
                async x => !x.Units.Any(y => y.Id == entityId)
                           && await _context.Units.AnyAsync(y => y.Id == entityId),
                x => x.UnitGroups.Add(new UnitGroupUnit
                {
                    UnitId = entityId
                })))
                return returnToEntity ? RedirectToAction("Edit", nameof(Unit), new { Area = "Admin", id = entityId}) : RedirectToAction("Edit", new { id = containerId });
            return RedirectToAction("Index");
        }
    }
    
    public class UnitGroupTranslationController : NoViewTranslationController<UnitGroup, UnitGroupTranslation>
    {
        public UnitGroupTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<UnitGroup> Queryable => _context.UnitGroups.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "UnitGroup", new {id});
    }
}
